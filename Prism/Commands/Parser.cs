using System.Collections.Generic;


namespace Specter.Debug.Prism.Commands;


public class Parser
{
	private List<Token> _tokens = [];
	private int _index;


	public CommandData? Parse(List<Token> tokens)
	{
		if (tokens.Count == 0)
			return null;

		_tokens = tokens;
		_index = 0;

		Location location = ParseLocation();
		Advance();
		List<object?> arguments = ParseArguments();

		return new(location, arguments);
	}


	private Location ParseLocation()
	{
		Location location = new();

		while (!AtEnd())
		{
			location.Names.Add(Advance().Lexeme);

			if (Peek().Type == TokenType.Colon)
				break;
		}

		return location;
	}


	private List<object?> ParseArguments()
	{
		List<object?> arguments = [];

		while (!AtEnd())
			arguments.Add(Advance().Value);

		return arguments;
	}


	private bool AtEnd() => _index >= _tokens.Count;

	private Token Advance() => _tokens[_index++];
	private Token Peek() => _tokens[_index];
}