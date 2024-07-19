using System.Collections.Generic;
using System.Linq;


namespace Specter.Color.Paint;


public partial class RulePainter
{
	private class Scanner
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

			bool empty = _tokens.Count == 0;

			Token? previous = empty ? null : _tokens.Last();
			Token? previousNonWhitespace = empty ? null : _tokens.FindLast(
				token => !char.IsWhiteSpace(token.Lexeme, 0)
			);
			
			token.Previous = previous;
			token.PreviousNonWhiteSpace = previousNonWhitespace;

			bool isThisTokenWhiteSpace = char.IsWhiteSpace(token.Lexeme, 0);

			if (previous is not null)
			{
				previous.Next = token;
				previous.NextNonWhiteSpace = !isThisTokenWhiteSpace ? token : null;
			}

			if (previousNonWhitespace is not null)
			{
				previousNonWhitespace.Next = token;
				previousNonWhitespace.NextNonWhiteSpace = !isThisTokenWhiteSpace ? token : null;
			}

			_tokens.Add(token);
		}



		private char Advance() => _source[Current++];

		private char Peek() => AtEnd() ? '\0' : _source[Current];

		private bool AtEnd() => Current >= _source.Length;
	}
}