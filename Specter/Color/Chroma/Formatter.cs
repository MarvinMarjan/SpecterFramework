using System.Text;
using System.Collections.Generic;


namespace Specter.Color.Chroma;


public static class Formatter
{
	public static string Format(List<IExpression> expressions)
	{
		StringBuilder builder = new();

		foreach (IExpression expression in expressions)
			builder.Append(expression.Stringify());

		return builder.ToString();
	}
}