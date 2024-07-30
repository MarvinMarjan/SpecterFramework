using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using Specter.Debug.Prism.Client;
using System.Linq;


namespace Specter.Debug.Prism.Server;


// TODO: add exception handling


public partial class PrismServer : TcpListener
{
	public ConcurrentDictionary<string, PrismClient> Clients { get; private set; } = [];


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

		foreach (var pair in Clients)
		{
			PrismClient client = pair.Value;

			if (client.Disconnected())
			{
				ClientRemove(client.Name);
				continue;
			}

			// FIXME: commands not working
			if (client.Stream.DataAvailable)
				datas.Add(client.ReadDataTransfer());
		}

		return datas;
	}



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

		Clients.TryAdd(registrationData.Name, new(registrationData.Name, client));

		Console.WriteLine($"Client registrated as {registrationData.Name}.");
	}
}