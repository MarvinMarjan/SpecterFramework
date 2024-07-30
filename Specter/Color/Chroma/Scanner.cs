using System.Collections.Generic;


namespace Specter.Color.Chroma;


public class Scanner
{
	private string _source = "";
	private int _start, _end;
	
	private int Current
	{
		get => _end;
		set => _end = value;
	}

	private List<Token> _tokens = [];


	public List<Token> Scan(string source)
	{
		_source = source;
		_start = _end = 0;

		_tokens.Clear();

		while (!AtEnd())
		{
			_start = _end;
			ScanToken();
		}

		return _tokens;
	}


	private void ScanToken()
	{
		char ch = Advance();

		switch (ch)
		{
			case '<': AddToken(TokenType.TagDelimeterLeft); break;
			case '>': AddToken(TokenType.TagDelimeterRight); break;
			case '(': AddToken(TokenType.LeftParen); break;
			case ')': AddToken(TokenType.RightParen); break;
			case '/': AddToken(TokenType.Slash); break;
			case ',': AddToken(TokenType.Comma); break;

			default:
				if (!char.IsLetterOrDigit(ch))
					AddToken(TokenType.Identifier);
				else
					Identifier();
				
				break;
		}
	}



	private void AddToken(TokenType type)
		=> AddToken(_source[_start .. _end], type);

	private void AddToken(string lexeme, TokenType type)
		=> _tokens.Add(new(lexeme, type, _start, _end));


	
	private void Identifier()
	{
		while (char.IsLetterOrDigit(Peek()))
			Advance();

		AddToken(TokenType.Identifier);
	}
	


	private char Advance() => _source[Current++];

	private char Peek() => AtEnd() ? '\0' : _source[Current];

	private bool AtEnd() => Current >= _source.Length;
}