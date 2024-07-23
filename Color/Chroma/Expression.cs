using System.Linq;
using System.Collections.Generic;


namespace Specter.Color.Chroma;


public interface IExpression
{
	public string Stringify();
}


public interface IExpressionConvertable
{
	IExpression ToExpression();
}



public class ExpressionConverter
{
	public static List<IExpression> ConvertAll(List<IExpressionConvertable> items)
		=> (from item in items select item.ToExpression()).ToList();
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