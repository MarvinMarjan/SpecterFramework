using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Specter.Debug.Prism.Client;
using Specter.Debug.Prism.Commands;


namespace Specter.Debug.Prism.Server;


// TODO: add exception handling


public struct RequestListener(Task task, CancellationTokenSource cancellationTokenSource)
{
	public Task Task { get; set; } = task;
	public CancellationTokenSource CancellationTokenSource { get; set; } = cancellationTokenSource;
}


public partial class PrismServer : TcpListener
{
	public ConcurrentDictionary<int, PrismClient> Clients { get; private set; }
	public ConcurrentDictionary<int, DataTransferStructure> Requests { get; private set; }

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


	private void ListenToNewClient()
	{
		TcpClient client = AcceptTcpClient();

		Console.WriteLine("New client connected. Waiting for registration...");

		DataTransferStructure? registrationData = new StreamReader(client.GetStream()).ReadDataTransfer();

		if (registrationData is not DataTransferStructure valid)
			return;

		Console.WriteLine($"Client registrated as {valid.Name}.");

		PrismClient prismClient = new(valid.Name, client);

		Clients.TryAdd(Clients.Count, prismClient);
		AddRequestListenerForClient(prismClient);
	}


	private void AddRequestListenerForClient(PrismClient client)
	{
		CancellationTokenSource tokenSource = new();

		_clientRequestListeners.TryAdd(client.Name, new(
			Task.Run(() => ListenForClientRequest(client, tokenSource.Token), tokenSource.Token),
			tokenSource
		));
	}


	private async Task ListenForClientRequest(PrismClient client, CancellationToken cancellationToken)
	{
		while (true)
		{
			DataTransferStructure? data = await client.Reader.ReadDataTransferAsync(cancellationToken);

			if (cancellationToken.IsCancellationRequested)
				break;

			if (data is DataTransferStructure valid)
				Requests.TryAdd(Requests.Count, valid);

			else
				ClientRemove(client.Name);
		}
	}
}