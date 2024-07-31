using System.Collections.Generic;

using Specter.Debug.Prism.Client;
using Specter.Debug.Prism.Commands.Definition;


namespace Specter.Debug.Prism.Commands;


public static class CommandRunner
{
	public static void Run(DataTransferStructure clientData)
	{
		List<Token> tokens = new Scanner().Scan(clientData.Command);
		CommandData? commandData = new Parser().Parse(tokens);

		if (commandData is CommandData valid)
			Run(clientData, valid);
	}


	public static void Run(DataTransferStructure clientData, CommandData commandData)
		=> CommandDefinition.Commands[commandData.Location.ToString()]
			.Execute(clientData, commandData.Arguments);
}