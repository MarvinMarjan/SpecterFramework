using System.Net;
using System.Net.Sockets;


namespace Specter.Debug.Prism.Server;


public class DebugServer(int port) : TcpListener(IPAddress.Loopback, port)
{
	
}