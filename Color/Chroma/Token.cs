namespace Specter.Color.Chroma;


public enum TokenType
{
	TagDelimeterLeft,
	TagDelimeterRight,
	LeftParen,
	RightParen,
	Slash,
	Comma,
	Identifier
}


public readonly struct Token(string lexeme, TokenType type, int start, int end)
{
	public string Lexeme { get; init; } = lexeme;
	public TokenType Type { get; init; } = type;

	public int Start { get; init; } = start;
	public int End { get; init; } = end;
}