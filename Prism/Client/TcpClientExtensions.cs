using System.Text;
using System.Net.Sockets;


namespace Specter.Debug.Prism.Client;


public static class TcpClientExtensions
{
	public static bool Disconnected(this TcpClient client)
	{
		try
		{
			client.WriteDataAsString("check");
			return false;
		}
		catch
		{
			return true;
		}
	}

	public static bool Disconnected(this PrismClient client)
		=> client.Tcp.Disconnected();


	public static ClientDataTransferStructure ReadDataTransfer(this PrismClient client)
		=> client.Tcp.ReadDataTransfer();

	public static ClientDataTransferStructure ReadDataTransfer(this TcpClient client)
		=> ClientDataTransferStructure.FromJson(client.ReadDataAsString());



	public static void WriteDataTransfer(this PrismClient client, ClientDataTransferStructure data)
		=> client.Tcp.WriteDataAsString(data.ToJson());

	public static void WriteDataTransfer(this TcpClient client, ClientDataTransferStructure data)
		=> client.WriteDataAsString(data.ToJson());



	public static string ReadDataAsString(this PrismClient client)
		=> client.Tcp.ReadDataAsString();

	public static string ReadDataAsString(this TcpClient client)
	{
		byte[] buffer = new byte[2048];
		int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);

		return Encoding.UTF8.GetString(buffer, 0, bytesRead);
	}



	public static void WriteDataAsString(this PrismClient client, string data)
		=> client.Tcp.WriteDataAsString(data);

	public static void WriteDataAsString(this TcpClient client, string data)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data ?? "");
		client.GetStream().Write(bytes, 0, bytes.Length);
	}
}