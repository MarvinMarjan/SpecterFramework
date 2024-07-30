using System;
using System.Net.Sockets;
using System.Text;

using Specter.Debug.Prism.Server;


namespace Specter.Tests;


public class PrismServerTesting
{
	public static void Main(string[] args)
	{
		const int PORT = 25000;

		DebugServer server = new(PORT);
		server.Start();

		Console.WriteLine($"Server listening at port {PORT}.");
		Console.WriteLine($"Waiting for a client...");
		
		TcpClient client = server.AcceptTcpClient();

		Console.WriteLine($"Client connected.");

		while (true)
		{
			NetworkStream stream = client.GetStream();



			if (!stream.DataAvailable)
				continue;

			byte[] buffer = new byte[256];
			int bytesRead = stream.Read(buffer, 0, buffer.Length);

			string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
		
			Console.WriteLine($"Client Message: {data}");
		}
	}
}