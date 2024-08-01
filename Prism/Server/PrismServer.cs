using System;
using System.Text.Json;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Specter.Debug.Prism.Client;
using Specter.Debug.Prism.Commands;
using Specter.Debug.Prism.Exceptions;


namespace Specter.Debug.Prism.Server;


// TODO: make server more flexibe, it looks like a program, not a framework.


// a Task that keeps running in loop waiting for a client request.
public struct RequestListener(Task task, CancellationTokenSource cancellationTokenSource)
{
	public Task Task { get; set; } = task;
	public CancellationTokenSource CancellationTokenSource { get; set; } = cancellationTokenSource;
}


public partial class PrismServer : TcpListener
{
	public ConcurrentDictionary<string, PrismClient> Clients { get; private set; }
	public ConcurrentDictionary<string, DataTransferStructure> Requests { get; private set; }

	private readonly Dictionary<string, RequestListener> _clientRequestListeners;

	private readonly Thread _listenToNewClientsThread;



	public PrismServer(int port)
		: base(IPAddress.Loopback, port)
	{
		ServerState.Server = this;

		Clients = [];
		Requests = [];

		// start server
		Start();

		Console.WriteLine($"Server listening at port {port}.");

		_listenToNewClientsThread = new(new ThreadStart(() => { while (true) ListenToNewClient(); }));
		_listenToNewClientsThread.Start();

		_clientRequestListeners = [];
	}



	public void ProcessRequests()
	{
		foreach (var (index, request) in Requests)
		{
			CommandRunner.Run(request);
			Requests.Remove(index, out _);
		}
	}



	public void AddClient(PrismClient client)
	{
		if (Clients.ContainsKey(client.Name))
			throw new ClientAlreadyExistsException(client.Name, "Can't add new client, it already exists.");

		Clients.TryAdd(client.Name, client);
		AddClientRequestListener(client);
	}

	public void RemoveClient(string clientName)
	{
		if (!Clients.ContainsKey(clientName))
			throw new ClientDoesNotExistsException(clientName, "Could not find client.");

		Clients.TryRemove(clientName, out PrismClient? client);
		RemoveClientRequestListener(clientName);

		Console.WriteLine($"Client {client?.Name} removed.");
	}


	public void AddClientRequestListener(PrismClient client)
	{
		CancellationTokenSource tokenSource = new();

		if (_clientRequestListeners.ContainsKey(client.Name))
			throw new ClientRequestListenerAlreadyExistsException(client.Name, "Can't add new client request listener, it already exists.");

		_clientRequestListeners.TryAdd(client.Name, new(
			Task.Run(() => ListenForClientRequest(client, tokenSource.Token), tokenSource.Token),
			tokenSource
		));
	}

	public void RemoveClientRequestListener(string clientName)
	{
		_clientRequestListeners.Remove(clientName, out RequestListener listener);

		listener.CancellationTokenSource.Cancel();
	}


	public void AddClientRequest(DataTransferStructure requestData)
	{
		if (Requests.ContainsKey(requestData.ClientName))
			throw new TooMuchRequestsException(requestData.ClientName, "Client has already sent a request.");

		Requests.TryAdd(requestData.ClientName, requestData);
	}



	public void Info(string message)
		=> Console.WriteLine($"Info: {message}");
	
	public void Warning(string message)
		=> Console.WriteLine($"Warning: {message}");

	public void Error(string message)
		=> Console.WriteLine($"Error: {message}");







	private void ListenToNewClient()
	{
		TcpClient client = AcceptTcpClient();

		Console.WriteLine("New client connected. Waiting for registration...");

		try
		{
			DataTransferStructure? registrationData = new StreamReader(client.GetStream()).ReadDataTransfer();
	
			if (registrationData is not DataTransferStructure validRegistrationData)
				throw new InvalidRegistrationDataException("Invalid registration data.");
	
			Console.WriteLine($"Client registrated as {validRegistrationData.ClientName}.");
	
			AddClient(new(validRegistrationData.ClientName, client));
		}
		catch (JsonException e)
		{
			throw new InvalidRegistrationDataException("JSON data is invalid.", e);
		}
	}


	private async Task ListenForClientRequest(PrismClient client, CancellationToken cancellationToken)
	{
		while (true)
		{
			DataTransferStructure? requestData = await client.Reader.ReadDataTransferAsync(cancellationToken);

			if (cancellationToken.IsCancellationRequested)
				break;

			if (requestData is DataTransferStructure validRequestData)
				AddClientRequest(validRequestData);
			else
				RemoveClient(client.Name);
		}
	}
}