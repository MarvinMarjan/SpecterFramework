using System;

using Specter.Util;


namespace Specter.Color.Paint;


public interface IRuleCondition
{
	public bool IsTrue(Token token);
	public bool IsFalse(Token token) => !IsTrue(token);
}


// TODO: receiving more than two argments ("param" keyword) probably is possible. try it later
public class LogicCondition(IRuleCondition left, IRuleCondition right, LogicCondition.LogicOperation operation) : IRuleCondition
{
	public enum LogicOperation
	{
		And,
		Or
	}


	public IRuleCondition Left { get; set; } = left;
	public IRuleCondition Right { get; set; } = right;
	public LogicOperation Operation { get; set; } = operation;


	public bool IsTrue(Token token) => Operation switch
	{
		LogicOperation.And => Left.IsTrue(token) && Right.IsTrue(token),
		LogicOperation.Or => Left.IsTrue(token) || Right.IsTrue(token),

		_ => false
	};
}


public class NextTokenIs(IRuleCondition condition) : IRuleCondition
{
	public IRuleCondition Condition { get; set; } = condition;
	public bool UseNextNonWhiteSpaceToken { get; set; } = true;


	public bool IsTrue(Token token)
	{
		Token? next = UseNextNonWhiteSpaceToken ? token.NextNonWhiteSpace : token.Next;
		
		return next is not null && Condition.IsTrue(next);
	}
}


public class PreviousTokenIs(IRuleCondition condition) : IRuleCondition
{
	public IRuleCondition Condition { get; set; } = condition;
	public bool UsePreviousNonWhiteSpaceToken { get; set; } = true;


	public bool IsTrue(Token token)
	{
		Token? previous = UsePreviousNonWhiteSpaceToken ? token.PreviousNonWhiteSpace : token.Previous;
		
		return previous is not null && Condition.IsTrue(previous);
	}
}


public class TokenIs(Func<Token, bool> predicate) : IRuleCondition
{
	public Func<Token, bool> Predicate { get; set; } = predicate;


	public bool IsTrue(Token token)
		=> Predicate(token);
}


public class TokenIsTarget(TokenTarget target) : IRuleCondition
{
	public TokenTarget Target { get; set; } = target;


	public bool IsTrue(Token token)
		=> Target.Match(token);
}


public class TokenIsWord()
	: TokenIs(token => char.IsLetter(token.Lexeme[0]) && token.Lexeme.IsAlphaNumeric());


public class TokenIsNumber()
	: TokenIs(token => double.TryParse(token.Lexeme, out _));