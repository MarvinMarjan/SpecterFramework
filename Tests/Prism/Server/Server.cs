using Specter.Debug.Prism.Commands;
using Specter.Debug.Prism.Server;


namespace Specter.Tests;


public class PrismServerTesting
{
	public static void Main()
	{
		PrismServer server = new(25000);

		while (true)
		{
			string data;

			if ((data = server.ReadDataFromClient()) == "")
				continue;

			CommandRunner.Run(data);
		}
	}
}