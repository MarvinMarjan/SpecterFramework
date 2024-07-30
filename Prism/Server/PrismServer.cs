using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

//using Specter.Debug.Prism.Client;


namespace Specter.Debug.Prism.Server;


// TODO: add exception handling


public class PrismServer : TcpListener
{
	// TODO: add a way to convert TcpClient into PrismClient
	// TODO: transfer data using JSON

	public TcpClient Client { get; set; }
	public NetworkStream ClientStream { get; set; }


	public PrismServer(int port)
		: base(IPAddress.Loopback, port)
	{
		ServerState.Server = this;

		Start();

		Console.WriteLine($"Server listening at port {port}.");
		Console.WriteLine($"Waiting for a client...");
		
		Client = AcceptTcpClient();
		ClientStream = Client.GetStream();
	
		Console.WriteLine($"Client connected.");
	}


	public string ReadDataFromClient()
	{
		if (!ClientStream.DataAvailable)
			return "";

		byte[] buffer = new byte[2048];
		int bytesRead = ClientStream.Read(buffer, 0, buffer.Length);

		return Encoding.UTF8.GetString(buffer, 0, bytesRead);
	}


	public void MsgInfo(string message)
		=> Console.WriteLine($"Info: {message}");
	
	public void MsgWarn(string message)
		=> Console.WriteLine($"Warning: {message}");

	public void MsgError(string message)
		=> Console.WriteLine($"Error: {message}");
}