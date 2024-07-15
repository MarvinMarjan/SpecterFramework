using System.Linq;


namespace Specter.Color.Paint;


/// <summary>
/// The base class for every paint rule.
/// </summary>
/// <param name="color"> The color to paint. </param>
public abstract class PaintRule(ColorObject color)
{
	public ColorObject Color { get; set; } = color;


	public abstract bool Match(Token token);
	public abstract bool Match(Token token, out PaintTarget target);
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


	private PaintTarget _lastTarget;


	public override bool Match(Token token) => Operator switch
	{
		LogicOperator.And => Left.Match(token, out _lastTarget) && Right.Match(token),
		LogicOperator.Or => Left.Match(token, out _lastTarget) || Right.Match(token),

		_ => false
	};

	public override bool Match(Token token, out PaintTarget target)
	{
		bool result = Match(token);
		
		target = _lastTarget with { Color = Color };

		return result;
	}
}



/// <summary>
/// A PaintRule that matches using equality.
/// </summary>
/// <param name="color"> The color to paint. </param>
/// <param name="sources"> The sources to check equality. </param>
/// <param name="equal"> Whether it should equal o not ('==' or '!='). </param>
public class EqualityRule(ColorObject color, string[] sources, bool equal = true)
	: PaintRule(color), ILogicRule
{
	public bool Equal { get; set; } = equal;
	public string[] Sources { get; set; } = sources;


	public override bool Match(Token token)
		=> Equal ? Sources.Contains(token.Lexeme) : !Sources.Contains(token.Lexeme);

	public override bool Match(Token token, out PaintTarget target)
	{
		target = new(Color, token.Start, token.End);

		return Match(token);
	}
}