using System.Threading;

namespace Specter.Color.Paint;


public class Token(string lexeme, int start, int end)
{
	public string Lexeme { get; init; } = lexeme;
    public Token? Next { get; set; }
    public Token? Previous { get; set; }
	public int Start { get; init; } = start;
	public int End { get; init; } = end;
}


public readonly struct TokenLexemeSet(params string[] set)
{
    public string[] Set { get; init; } = set;


    public bool Match(Token token)
    {
        Token? currentToken = token;
        bool matched = true;

        foreach (string tokenLexeme in Set)
        {
            if (currentToken is null || currentToken.Lexeme != tokenLexeme)
            {
                matched = false;
                break;
            }

            currentToken = currentToken.Next;
        }

        return matched;
    }


    public static implicit operator TokenLexemeSet(string source) => new([source]);
    public static implicit operator TokenLexemeSet(string[] source) => new(source);
}