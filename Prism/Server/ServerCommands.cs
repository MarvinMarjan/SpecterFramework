using System;
using Specter.Debug.Prism.Client;


namespace Specter.Debug.Prism.Server;


public partial class PrismServer
{
	public void ClientRemove(string clientName)
	{
		foreach (var (index, client) in Clients)
			if (client.Name == clientName)
				ClientRemove(index);
	}

	public void ClientRemove(int index)
	{
		Clients.TryRemove(index, out PrismClient? client);
		_clientRequestListeners.Remove(client?.Name ?? "", out RequestListener listener);

		listener.CancellationTokenSource.Cancel();

		Console.WriteLine($"Client {client?.Name} ({index}) removed.");
	}



	public void MsgInfo(string message)
		=> Console.WriteLine($"Info: {message}");
	
	public void MsgWarn(string message)
		=> Console.WriteLine($"Warning: {message}");

	public void MsgError(string message)
		=> Console.WriteLine($"Error: {message}");
}