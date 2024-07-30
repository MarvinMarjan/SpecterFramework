using System.Collections.Generic;
using System.Text;

using Specter.Terminal.Input;


namespace Specter.Color.Paint;


/// <summary>
/// Paints using one or more paint rules.
/// </summary>
/// <param name="rules"> The paint rules to use. </param>
public partial class RulePainter(List<PaintRule> rules) : Painter
{
	public List<PaintRule> Rules { get; set; } = rules;

	private PaintingState _state = new(ColorObject.None);
	public ref PaintingState State => ref _state;

	/// <summary>
	/// The optional cursor to draw.
	/// </summary>
	public Cursor? Cursor { get; set; } = null;



	public override string Paint(string source)
	{
		List<Token> tokens = new RulePainterScanner().Scan(source);
		
		return PaintTokens(tokens);
	}


	private string PaintTokens(List<Token> tokens)
	{
		StringBuilder builder = new();
		bool cursorDrawed = false;
		
		foreach (Token token in tokens)
		{
			foreach (PaintRule rule in Rules)
			{
				if (State.ShouldIgnoreRuleMatching)
					break;

				if (rule.Match(ref _state, token))
					break;
			}

			builder.Append(DrawToken(token, ref cursorDrawed));

			State.Update(token);
		}

		if (!cursorDrawed)
			builder.Append(Cursor?.GetCursorAtEnd() ?? "");

		State.FullReset();

		return builder.ToString();
	}


	private string DrawToken(Token token, ref bool cursorDrawed)
	{
		string drawnToken = DrawCursorIfInsideToken(
			Cursor ?? new(), token, State.Color,
			CursorAtToken(token), out bool drawed
		);

		// once set to true, never turns to false again
		if (drawed)
			cursorDrawed = true;

		return drawnToken;
	}


	private bool CursorAtToken(Token token)
		=> Cursor?.Index >= token.Start && Cursor?.Index < token.End;


	private static string DrawCursorIfInsideToken(Cursor cursor, Token token, ColorObject? tokenColor, bool inside, out bool cursorDrawed)
	{
		StringBuilder builder = new();

		if (cursorDrawed = inside)
			builder.Append(DrawCursorAtToken(cursor, token, tokenColor));
		else
			builder.Append(tokenColor?.Paint(token.Lexeme) ?? token.Lexeme);

		return builder.ToString();
	}


	private static string DrawCursorAtToken(Cursor cursor, Token token, ColorObject? tokenColor = null)
	{
		StringBuilder builder = new(tokenColor?.AsSequence() ?? "");

		for (int i = 0; i < token.Lexeme.Length; i++)
		{
			char ch = token.Lexeme[i];

			if (cursor.Index == i + token.Start)
				builder.Append(cursor.DrawTo(ch, tokenColor));
			else
				builder.Append(ch);
		}

		builder.Append(ColorValue.Reset.AsSequence());

		return builder.ToString();
	}
}
