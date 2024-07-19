namespace Specter.Color.Paint;


public class Token(string lexeme, int start, int end)
{
	public string Lexeme { get; init; } = lexeme;
	
	public bool IsWhiteSpace => char.IsWhiteSpace(Lexeme, 0);

	public Token? Next { get; set; }
	public Token? Previous { get; set; }
	public Token? NextNonWhiteSpace { get; set; }
	public Token? PreviousNonWhiteSpace { get; set; }

	public int Start { get; init; } = start;
	public int End { get; init; } = end;
}


public readonly struct TokenTarget(params string[] set)
{
	public string[] Set { get; init; } = set;

	public bool ShouldIgnoreWhitespace { get; init; } = true;


	public bool Match(Token token)
	{
		Token? currentToken = token;
		bool matched = true;

		foreach (string tokenLexeme in Set)
		{
			if (!MatchSingle(currentToken, tokenLexeme))
			{
				matched = false;
				break;
			}

			currentToken = currentToken?.Next;
		}

		return matched;
	}


	private bool MatchSingle(Token? token, string lexeme)
	{
		if (token is null)
			return false;

		bool matchLexeme = token.Lexeme == lexeme || lexeme.Length == 0;

		return matchLexeme && (!ShouldIgnoreWhitespace || !token.IsWhiteSpace);
	}


	public static implicit operator TokenTarget(string source) => new([source]);
	public static implicit operator TokenTarget(string[] source) => new(source);
}