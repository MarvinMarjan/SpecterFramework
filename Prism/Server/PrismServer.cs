using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

using Specter.Debug.Prism.Client;
using System.Net.WebSockets;


namespace Specter.Debug.Prism.Server;


// TODO: add exception handling


public class PrismServer : TcpListener
{
	public List<PrismClient> Clients { get; init; } = [];


	private readonly Thread _listenToNewClientsThread;


	public PrismServer(int port)
		: base(IPAddress.Loopback, port)
	{
		ServerState.Server = this;

		// start server
		Start();

		Console.WriteLine($"Server listening at port {port}.");

		_listenToNewClientsThread = new(ListenToNewClients);
		_listenToNewClientsThread.Start();
	}


	public List<ClientDataTransferStructure> ReadAllDataTransfers()
	{
		List<ClientDataTransferStructure> datas = [];

		for (int i = Clients.Count - 1; i >= 0; i--)
		{
			PrismClient client = Clients[i];

			if (client.Stream.DataAvailable)
				datas.Add(client.ReadDataTransfer());
		}

		return datas;
	}


	public void MsgInfo(string message)
		=> Console.WriteLine($"Info: {message}");
	
	public void MsgWarn(string message)
		=> Console.WriteLine($"Warning: {message}");

	public void MsgError(string message)
		=> Console.WriteLine($"Error: {message}");



	private void ListenToNewClients()
	{
		while (true)
			ListenToNewClient();
	}


	private void ListenToNewClient()
	{
		TcpClient client = AcceptTcpClient();

		Console.WriteLine("New client connected. Waiting for registration...");

		ClientDataTransferStructure registrationData = client.ReadDataTransfer();

		Clients.Add(new(registrationData.Name, client));

		Console.WriteLine($"Client registrated as {registrationData.Name}.");
	}
}