using System.Collections.Generic;
using Specter.Debug.Prism.Client;
using Specter.Debug.Prism.Server;


namespace Specter.Debug.Prism.Commands.Definition;


public class MsgInfo : ICommand
{
	public void Execute(DataTransferStructure clientData, List<object?> args)
		=> ServerState.Server?.Info(args[0] as string ?? "");
}

public class MsgWarn : ICommand
{
	public void Execute(DataTransferStructure clientData, List<object?> args)
		=> ServerState.Server?.Warning(args[0] as string ?? "");
}

public class MsgError : ICommand
{
	public void Execute(DataTransferStructure clientData, List<object?> args)
		=> ServerState.Server?.Error(args[0] as string ?? "");
}