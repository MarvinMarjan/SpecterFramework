using System.Collections.Generic;
using Specter.Debug.Prism.Client;
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
			List<ClientDataTransferStructure> datas = server.ReadAllDataTransfers();

			foreach (ClientDataTransferStructure data in datas)
				CommandRunner.Run(data);
		}
	}
}