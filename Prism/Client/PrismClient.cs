using System.Net.Sockets;


namespace Specter.Debug.Prism.Client;


public class PrismClient
{
	public string Name { get; init; }

	public NetworkStream Stream { get; init; }
	public TcpClient Tcp { get; init; }


	public PrismClient(string name, int port)
		: this(name, new TcpClient("localhost", port))
	{
		Tcp.WriteDataAsString(ToDataTransferStructure().ToJson());
	}


	public PrismClient(string name, TcpClient client)
	{
		Name = name;
		Tcp = client;
		Stream = Tcp.GetStream();
	}


	public ClientDataTransferStructure ToDataTransferStructure()
		=> new(Name, "");


	public void WriteDataTransfer()
		=> Tcp.WriteDataTransfer(ToDataTransferStructure());

	public void WriteCommandRequest(string command)
		=> Tcp.WriteDataTransfer(ToDataTransferStructure() with { Command = command });
}