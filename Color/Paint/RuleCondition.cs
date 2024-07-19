namespace Specter.Color.Paint;


public interface IRuleCondition
{
	public bool IsTrue(Token token);
	public bool IsFalse(Token token) => !IsTrue(token);
}


public class NextTokenIs(TokenTarget set) : IRuleCondition
{
	public TokenTarget Set { get; set; } = set;
	public bool UseNextNonWhiteSpaceToken { get; set; } = true;


	public bool IsTrue(Token token)
	{
		Token? next = UseNextNonWhiteSpaceToken ? token.NextNonWhiteSpace : token.Next;
		
		return next is not null && Set.Match(next);
	}
}


public class PreviousTokenIs(TokenTarget set) : IRuleCondition
{
	public TokenTarget Set { get; set; } = set;
	public bool UsePreviousNonWhiteSpaceToken { get; set; } = true;


	public bool IsTrue(Token token)
	{
		Token? previous = UsePreviousNonWhiteSpaceToken ? token.PreviousNonWhiteSpace : token.Previous;
		
		return previous is not null && Set.Match(previous);
	}
}