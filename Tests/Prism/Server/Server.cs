using Specter.Debug.Prism.Server;


namespace Specter.Tests;


public class PrismServerTesting
{
	public static void Main()
	{
		PrismServer server = new(25000);

		while (true)
			server.ProcessRequests();
	}
}