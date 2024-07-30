using System.Collections.Generic;
using Specter.Debug.Prism.Client;
using Specter.Debug.Prism.Server;


namespace Specter.Debug.Prism.Commands.Definition;


public class MsgInfo : ICommand
{
	public void Execute(ClientDataTransferStructure clientData, List<object?> args)
		=> ServerState.Server?.MsgInfo(args[0] as string ?? "");
}

public class MsgWarn : ICommand
{
	public void Execute(ClientDataTransferStructure clientData, List<object?> args)
		=> ServerState.Server?.MsgWarn(args[0] as string ?? "");
}

public class MsgError : ICommand
{
	public void Execute(ClientDataTransferStructure clientData, List<object?> args)
		=> ServerState.Server?.MsgError(args[0] as string ?? "");
}