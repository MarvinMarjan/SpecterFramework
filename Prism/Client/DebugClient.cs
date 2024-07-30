using System.Net.Sockets;


namespace Specter.Debug.Prism.Client;


public class DebugClient(int port) : TcpClient("localhost", port)
{
	
}