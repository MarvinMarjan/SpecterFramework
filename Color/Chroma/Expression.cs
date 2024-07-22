namespace Specter.Color.Chroma;


public interface IExpression
{
	public string Stringify();
}


public interface IExpressionConvertable
{
	IExpression ToExpression();
}



public class FormatExpression(ColorObject color) : IExpression
{
	public ColorObject Color { get; set; } = color;


	public string Stringify()
		=> Color.AsSequence();
}


public class TextExpression(string text) : IExpression
{
	public string Text { get; set; } = text;


	public string Stringify()
		=> Text;
}