using System.Collections.Generic;

using Specter.Debug.Prism.Commands.Definition;


namespace Specter.Debug.Prism.Commands;


public static class CommandRunner
{
	public static void Run(string command)
	{
		List<Token> tokens = new Scanner().Scan(command);
		CommandData? commandData = new Parser().Parse(tokens);

		if (commandData is CommandData valid)
			Run(valid);
	}


	public static void Run(CommandData commandData)
		=> CommandDefinition.Commands[commandData.Location.ToString()].Execute(commandData.Arguments);
}