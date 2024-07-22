using System.Collections.Generic;


namespace Specter.Color.Chroma;


public struct FormatTag(string fg, string bg, string mode)
{
	public string Foreground { get; set; } = fg;
	public string Background { get; set; } = bg;
	public string Mode { get; set; } = mode;

	public bool ResetTag { get; set; }


	public FormatTag() : this("", "", "")
	{
		ResetTag = true;
	}


	public readonly ColorObject ToColorObject()
	{
		if (ResetTag)
			return ColorValue.Reset;

		ColorObject color = ColorObject.None;

		color.Foreground = Foreground == "_" ? null : ColorTable.GetColor(Foreground, ColorLayer.Foreground);
		color.Background = Background == "_" ? null : ColorTable.GetColor(Background, ColorLayer.Background);
		color.Mode       = Mode       == "_" ? null : ColorTable.GetMode(Mode);

		return color;
	}
}


public class Parser
{
	private List<Token> _tokens = [];
	private int _index;


	public List<IExpression> Parse(List<Token> tokens)
	{
		List<IExpression> expressions = [];
		
		_tokens = tokens;

		while (!IsAtEnd())
		{
			Token current = Advance();

			switch (current.Type)
			{
			case TokenType.TagDelimeterLeft:
				expressions.Add(new FormatExpression(ParseTag().ToColorObject()));
				Advance(); // advance tag delimiter right advance
				break;

			default:
				expressions.Add(new TextExpression(current.Lexeme));
				break;
			}
		}

		return expressions;
	}


	private FormatTag ParseTag()
	{
		List<Token> tokens = [];
		string[] arguments = ["_", "_", "_"];
		Token tagStart = Previous();

		while (Current().Type != TokenType.TagDelimeterRight && !IsAtEnd())
			tokens.Add(Advance());

		if (IsAtEnd())
			throw new ChromaException("Unclosed tag.", tagStart);

		// remove unnecessary characters
		tokens.RemoveAll(token => char.IsWhiteSpace(token.Lexeme[0]));

		if (tokens.Count == 1 && tokens[0].Lexeme == "/")
			return new(); // reset tag

		if (tokens.Count < 1)
			throw new ChromaException("Expected at least 1 argument.", tagStart);

		if (tokens.Count > 3)
			throw new ChromaException($"Expected a max of 3 arguments, got {tokens.Count}", tagStart);

		for (int i = 0; i < tokens.Count; i++)
			arguments[i] = tokens[i].Lexeme;

		return new(arguments[0], arguments[1], arguments[2]);
	}


	private bool IsAtEnd() => _index >= _tokens.Count;

	private Token Advance() => _tokens[_index++];
	private Token Current() => _tokens[_index];
	private Token Previous() => _tokens[_index - 1];
}