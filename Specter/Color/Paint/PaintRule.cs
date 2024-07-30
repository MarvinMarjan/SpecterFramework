namespace Specter.Color.Paint;


/// <summary>
/// The base class for every paint rule.
/// </summary>
/// <param name="color"> The color to paint. </param>
public abstract class PaintRule(ColorObject color)
{
	public ColorObject Color { get; set; } = color;
	public bool IgnoreWhiteSpace { get; set; } = true;


	public abstract bool Match(ref PaintingState state, Token token);
}



/// <summary>
/// A rule that matches when its condition is true.
/// </summary>
/// <param name="color"> The color to paint. </param>
/// <param name="condition"> The condition. </param>
public class ConditionalRule(ColorObject color, IRuleCondition? condition)
	: PaintRule(color)
{
	public IRuleCondition? Condition { get; set; } = condition;

	public bool ShouldIgnoreWhitespace { get; set; } = true;


	public override bool Match(ref PaintingState state, Token token)
	{
		if (ShouldIgnoreWhitespace && token.IsWhiteSpace)
			return false;

		bool matched = Condition?.IsTrue(token) ?? true;

		if (matched)
		{
			state.Color = Color;
			state.PaintLength = 1;
		}

		return matched;
	}
}



/// <summary>
/// A PaintRule that matches using equality.
/// </summary>
/// <param name="color"> The color to paint. </param>
/// <param name="sources"> The sources to check equality. </param>
/// <param name="equal"> Whether it should equal o not ('==' or '!='). </param>
public class EqualityRule(

	ColorObject color,
	TokenTarget[] sources,
	IRuleCondition? condition = null,
	bool equal = true

) : PaintRule(color)
{
	public TokenTarget[] Sources { get; set; } = sources;
	public IRuleCondition? Condition { get; set; } = condition;
	public bool Equal { get; set; } = equal;

	public int? ExtraPaintLength { get; set; }


	public override bool Match(ref PaintingState state, Token token)
	{
		bool matched = false;
		int paintLength = 0;

		bool ConditionIsTrue = Condition?.IsTrue(token) ?? true;

		foreach (TokenTarget set in Sources)
		{
			if (!set.Match(token) || !ConditionIsTrue)
				continue;

			paintLength = set.Set.Length;
			matched = true;
			
			break;
		}
			
		if (matched)
		{
			state.Color = Color;
			state.PaintLength = paintLength + (ExtraPaintLength ?? 0);
		}
		
		return matched;
	}
}



/// <summary>
/// A rule that matches for every target between two other targets.
/// </summary>
/// <param name="color"> The color to paint. </param>
/// <param name="left"> The left sided target. </param>
/// <param name="right"> The right sided target. </param>
public class BetweenRule(ColorObject color, TokenTarget left, TokenTarget right)
	: PaintRule(color)
{
	public TokenTarget Left { get; set; } = left;
	public TokenTarget Right { get; set; } = right;


	public override bool Match(ref PaintingState state, Token token)
	{
		bool matched = Left.Match(token);

		if (matched)
		{
			state.PaintUntilToken = Right; // paints until the right sided target
			state.Color = Color;
			state.IgnoreCurrentToken = true;
		}

		return matched;
	}
}



/// <summary>
/// Uses a custom condition to match.
/// </summary>
/// <param name="color"> The color to paint. </param>
/// <param name="predicate"> The function that checks whether it matches or not. </param>
public class CustomMatchRule(ColorObject color, CustomMatchRule.RuleFunc predicate)
	: PaintRule(color)
{
	public delegate bool RuleFunc(Token token);

	public RuleFunc Predicate { get; set; } = predicate;


	public override bool Match(ref PaintingState state, Token token)
	{
		bool matched = Predicate(token);

		if (matched)
		{
			state.Color = Color;
			state.PaintLength = 1;
		}

		return matched;
	}
}



/// <summary>
/// Full custom behaviour.
/// </summary>
/// <param name="color"> The color to paint. </param>
/// <param name="matcher"> The matcher function. </param>
public class CustomRule(ColorObject color, CustomRule.RuleFunc matcher)
	: PaintRule(color)
{
	public delegate bool RuleFunc(ref PaintingState state, Token token);

	public RuleFunc Matcher { get; set; } = matcher;


	public override bool Match(ref PaintingState state, Token token)
		=> Matcher(ref state, token);
}