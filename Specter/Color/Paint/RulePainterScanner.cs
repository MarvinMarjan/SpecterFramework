using System.Collections.Generic;
using System.Linq;


namespace Specter.Color.Paint;


public class RulePainterScanner
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

		if (char.IsLetter(ch))
			Identifier();
		else
			AddToken();
	}


	private void Identifier()
	{
		while (char.IsLetterOrDigit(Peek()))
			Advance();

		AddToken();
	}



	private void AddToken()
		=> AddToken(_source[_start .. _end]);

	private void AddToken(string lexeme)
	{
		Token token = new(lexeme, _start, _end);

		SetTokenNeighbors(token);

		_tokens.Add(token);
	}


	private void SetTokenNeighbors(Token token)
	{
		bool empty = _tokens.Count == 0;

		Token? previous = empty ? null : _tokens.Last();
		Token? previousNonWhitespace = _tokens.FindLast(token => !token.IsWhiteSpace);

		token.Previous = previous;
		token.PreviousNonWhiteSpace = previousNonWhitespace;

		if (previous is not null)
		{
			previous.Next = token;
			previous.NextNonWhiteSpace = !token.IsWhiteSpace ? token : null;
		}

		if (previousNonWhitespace is not null)
		{
			previousNonWhitespace.Next = token;
			previousNonWhitespace.NextNonWhiteSpace = !token.IsWhiteSpace ? token : null;
		}
	}



	private char Advance() => _source[Current++];

	private char Peek() => AtEnd() ? '\0' : _source[Current];

	private bool AtEnd() => Current >= _source.Length;
}