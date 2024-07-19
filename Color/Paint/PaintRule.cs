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
/// Rules that don't inherit from this aren't able to be used as operands in 
/// LogicRule.
/// ! Probably useless. Maybe should be removed.
/// </summary>
public interface ILogicRule
{}

public class LogicRule<TRule>(ColorObject color, TRule left, TRule right, LogicRule<TRule>.LogicOperator @operator)
	: PaintRule(color)
		where TRule : PaintRule, ILogicRule
{
	public enum LogicOperator
	{
		And,
		Or
	}


	public TRule Left { get; set; } = left;
	public TRule Right { get; set; } = right;

	public LogicOperator Operator { get; set; } = @operator;


	public override bool Match(ref PaintingState state, Token token) => Operator switch
	{
		LogicOperator.And => Left.Match(ref state, token) && Right.Match(ref state, token),
		LogicOperator.Or => Left.Match(ref state, token) || Right.Match(ref state, token),

		_ => false
	};
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

) : PaintRule(color), ILogicRule
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
			state.PaintUntilToken = Right;
			state.Color = Color;
			state.IgnoreCurrentToken = true;
		}

		return matched;
	}
}