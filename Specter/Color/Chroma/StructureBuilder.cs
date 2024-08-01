using System.Linq;
using System.Collections.Generic;


namespace Specter.Color.Chroma;


public class StructureBuilder
{
	private int _index;
	private List<Token> _tokens = [];


	public List<IExpressionConvertable> BuildExpressionConvertableStructures(List<Token> tokens)
	{
		List<IExpressionConvertable> structures = [];

		_tokens = tokens;

		while (!IsAtEnd())
		{
			Token current = Advance();

			switch (current.Type)
			{
			case TokenType.TagDelimeterLeft:
				structures.Add(BuildTag());
				break;

			default:
				structures.Add(new IdentifierStructure(current.Lexeme));
				break;
			}
		}

		return structures;
	}


	private FormatTagStructure BuildTag()
	{
		List<INotationConvertableStructure> tagStructures = [];

		while (Peek().Type != TokenType.TagDelimeterRight)
		{
			Token current = Advance();

			// ignore white space characters
			if (char.IsWhiteSpace(current.Lexeme[0]))
				continue;

			switch (current.Type)
			{
			case TokenType.Identifier:
				tagStructures.Add(new IdentifierStructure(current.Lexeme));
				break;

			case TokenType.LeftParen:
				tagStructures.Add(BuildRGB());
				break;

			case TokenType.Slash:
				Advance();
				return new FormatTagStructure() { ResetTag = true };
			}
		}

		int tagSize = tagStructures.Count;

		for (int i = 0; i < 3 - tagSize; i++)
			tagStructures.Add(INotationConvertableStructure.DefaultNotation);

		Advance();

		return new(tagStructures[0], tagStructures[1], tagStructures[2]);
	}


	private RGBStructure BuildRGB()
	{
		List<Token> rgbTokens = [];
		HighlightTarget target = new(Previous(), FindFirstTokenWithLexeme(")"));

		while (Peek().Type != TokenType.RightParen)
			rgbTokens.Add(Advance());

		// remove commas and white spaces
		rgbTokens.RemoveAll(token => token.Type == TokenType.Comma);
		rgbTokens.RemoveAll(token => char.IsWhiteSpace(token.Lexeme[0]));

		if (rgbTokens.Count > 3)
			throw new ChromaException(target, $"RGB can't have more than 3 channels, got {rgbTokens.Count}");

		if (rgbTokens.Count == 0)
			throw new ChromaException(target, $"RGB must have at least one channel.");

		string rgbNotation = string.Join(' ', from rgb in rgbTokens select rgb.Lexeme);

		if (!Notation.IsRGBNotation(rgbNotation, out byte[]? values))
			throw new ChromaException(target, $"Invalid RGB notation.");

		Advance();

		return new(values?[0] ?? 0, values?[1] ?? 0, values?[2] ?? 0);
	}


	private Token FindFirstTokenWithLexeme(string lexeme)
		=> _tokens[_tokens.FindIndex(_index, token => token.Lexeme == lexeme)];


	private bool IsAtEnd() => _index >= _tokens.Count;

	private Token Advance() => _tokens[_index++];
	private Token Peek() => _tokens[_index];
	private Token Previous() => _tokens[_index - 1];

}
