using System.Collections.Generic;
using Specter.ANSI;
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
/// Paints using one or more PaintRule.
/// </summary>
/// <param name="rules"> The paint rules to use. </param>
public partial class RulePainter(List<PaintRule> rules) : Painter
{
	public List<PaintRule> Rules { get; set; } = rules;
	public Cursor? Cursor { get; set; } = null;



	public override string Paint(string source)
	{
		List<Token> tokens = new Scanner().Scan(source);
		List<PaintTarget> targets = MatchRules(tokens);

		targets.Reverse();

		return PaintStringUsingTargets(source, targets);
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


	private string PaintStringUsingTargets(string source, List<PaintTarget> targets)
	{
		if (Cursor is not null && targets.Count == 0)
			source = Cursor.DrawTo(source); 

		foreach (PaintTarget target in targets)
		{
			// FIXME: not working
			if (Cursor?.Index >= target.Start && Cursor?.Index <= target.End)
			{
				source = Cursor.DrawTo(source);
				continue;
			}

			source = source.Insert(target.End, EscapeCodes.Reset);
			source = source.Insert(target.Start, target.Color.AsSequence());
		}

		return source;
	}
}
