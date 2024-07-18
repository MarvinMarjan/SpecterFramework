namespace Specter.Color.Paint;


/// <summary>
/// The base class for every paint rule.
/// </summary>
/// <param name="color"> The color to paint. </param>
public abstract class PaintRule(ColorObject color)
{
	public ColorObject Color { get; set; } = color;


	public abstract bool Match(ref PaintingState state, Token token);
}


/// <summary>
/// Rules that don't inherit from this aren't able to be used as operands in 
/// LogicRule.
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
public class EqualityRule(ColorObject color, TokenLexemeSet[] sources, bool equal = true)
	: PaintRule(color), ILogicRule
{
	public bool Equal { get; set; } = equal;
	public TokenLexemeSet[] Sources { get; set; } = sources;

	public int? ExtraPaintLength { get; set; }


	public override bool Match(ref PaintingState state, Token token)
	{
		bool matched = false;
		int paintLength = 0;

		foreach (TokenLexemeSet set in Sources)
		{
			if (!set.Match(token))
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



public class BetweenRule(ColorObject color, TokenLexemeSet left, TokenLexemeSet right)
	: PaintRule(color)
{
	public TokenLexemeSet Left { get; set; } = left;
	public TokenLexemeSet Right { get; set; } = right;


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