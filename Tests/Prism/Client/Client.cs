using System;

using Specter.Debug.Prism.Client;
using Specter.Debug.Prism.Server;


namespace Specter.Tests;


public class ClientServerTesting
{
	public static void Main(string[] args)
	{
		PrismClient client = new(args.Length > 0 ? args[0] : "TestClient", ServerState.Port);
		
		while (true)
			client.WriteCommandRequest(Console.ReadLine() ?? "");
	}
}