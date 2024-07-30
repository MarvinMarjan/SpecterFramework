using System;

using Specter.Debug.Prism.Client;


namespace Specter.Tests;


public class ClientServerTesting
{
	public static void Main()
	{
		PrismClient client = new(25000);
		
		while (true)
			client.WriteDataToServer(Console.ReadLine() ?? "");
	}
}