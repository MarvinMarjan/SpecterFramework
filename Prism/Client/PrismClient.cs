using System;
using System.Text;
using System.Net.Sockets;


namespace Specter.Debug.Prism.Client;


public class PrismClient : TcpClient
{
	public NetworkStream Stream { get; set; }


	public PrismClient(int port)
		: base("localhost", port)
	{
		Console.WriteLine($"Client connected at port {port}.");
		Console.WriteLine($"Start writing:\n");

		Stream = GetStream();
	}


	public void WriteDataToServer(string data)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data ?? "");
		Stream.Write(bytes, 0, bytes.Length);
	}
}