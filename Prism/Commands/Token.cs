namespace Specter.Debug.Prism.Commands;


public enum TokenType
{
	Identifier, Colon, Value
}


public readonly struct Token(string lexeme, int start, int end, object? value, TokenType type)
{
	public string Lexeme { get; init; } = lexeme;
	public int Start { get; init; } = start;
	public int End { get; init; } = end;
	public object? Value { get; init; } = value;
	public TokenType Type { get; init; } = type;
}