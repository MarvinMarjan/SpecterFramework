using System;
using System.Collections.Generic;
using Specter.Debug.Prism.Client;


namespace Specter.Debug.Prism.Server;


public partial class PrismServer
{
	public void ClientRemove(string clientName)
	{
		Clients.TryRemove(clientName, out _);

		Console.WriteLine($"Client {clientName} disconnected.");
	}



	public void MsgInfo(string message)
		=> Console.WriteLine($"Info: {message}");
	
	public void MsgWarn(string message)
		=> Console.WriteLine($"Warning: {message}");

	public void MsgError(string message)
		=> Console.WriteLine($"Error: {message}");
}