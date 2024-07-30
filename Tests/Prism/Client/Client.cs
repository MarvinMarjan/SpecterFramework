using System;
using System.Net.Sockets;
using System.Text;

using Specter.Debug.Prism.Client;


namespace Specter.Tests;


public class ClientServerTesting
{
	public static void Main(string[] args)
	{
		const int PORT = 25000;

		DebugClient client = new(PORT);

		Console.WriteLine($"Client connected at port {PORT}.");
		Console.WriteLine($"Start writing:\n");
		
		while (true)
		{
			string? data = Console.ReadLine();
			byte[] bytes = Encoding.UTF8.GetBytes(data ?? "");

			NetworkStream stream = client.GetStream();
			stream.Write(bytes, 0, bytes.Length);
		}
	}
}