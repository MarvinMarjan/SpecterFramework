using System.Collections.Generic;
using System.Text;

using Specter.Terminal.Input;


namespace Specter.Color.Paint;


/// <summary>
/// Represents targets of a string to paint.
/// </summary>
/// <param name="color"> The color to paint. </param>
/// <param name="start"> The start index of the target. </param>
/// <param name="end"> The end index of the target. </param>
public readonly struct PaintTarget(ColorObject color, int start, int end)
{
	public ColorObject Color { get; init; } = color;

	public int Start { get; init; } = start;
	public int End { get; init; } = end;
}


/// <summary>
/// Paints using one or more paint rules.
/// </summary>
/// <param name="rules"> The paint rules to use. </param>
public partial class RulePainter(List<PaintRule> rules) : Painter
{
	public List<PaintRule> Rules { get; set; } = rules;

	/// <summary>
	/// The optional cursor to draw.
	/// </summary>
	public Cursor? Cursor { get; set; } = null;



	public override string Paint(string source)
	{
		List<Token> tokens = new Scanner().Scan(source);
		List<PaintTarget> targets = MatchRules(tokens);

		targets.Reverse();

		return PaintTokensUsingTargets(tokens, targets);
	}

	
	private List<PaintTarget> MatchRules(List<Token> tokens)
	{
		List<PaintTarget> targets = [];

		foreach (Token token in tokens)
			foreach (PaintRule rule in Rules)
			{
				if (!rule.Match(token, out PaintTarget target))
					continue;

				targets.Add(target);

				break;
			}

		return targets;
	}


	private string PaintTokensUsingTargets(List<Token> tokens, List<PaintTarget> targets)
	{
		StringBuilder builder = new();
		
		PaintTarget? targetMatched;
		bool cursorDrawed = false;

		foreach (Token token in tokens)
		{
			targetMatched = null;

			foreach (PaintTarget target in targets)
			{
				// If the Start and End indexes of the token are the same as the target, paint the token.
				if (token.Start != target.Start || token.End != target.End)
					continue;

				targetMatched = target;
				break;
			}
			
			builder.Append(DrawCursorIfInsideToken(
				Cursor ?? new(), token, targetMatched?.Color,
				CursorAtToken(token), out bool drawed
			));

			if (drawed)
				cursorDrawed = true;
		}

		if (Cursor is not null && !cursorDrawed)
			builder.Append(Cursor.GetCursorAtEnd());

		return builder.ToString();
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
