using System.Collections.Generic;

using Specter.Debug.Prism.Client;
using Specter.Debug.Prism.Server;


namespace Specter.Debug.Prism.Commands.Definition;


public class ClientRemove : ICommand
{
	public void Execute(DataTransferStructure clientData, List<object?> args)
		=> ServerState.Server?.ClientRemove(args[0] as string ?? "");
}