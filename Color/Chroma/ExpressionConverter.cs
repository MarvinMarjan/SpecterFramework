using System.Linq;
using System.Collections.Generic;


namespace Specter.Color.Chroma;


public class ExpressionConverter
{
	public static List<IExpression> Convert(List<IExpressionConvertable> items)
		=> (from item in items select item.ToExpression()).ToList();
}