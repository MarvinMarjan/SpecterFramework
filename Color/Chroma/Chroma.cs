namespace Specter.Color.Chroma;


public static class Chroma
{
	public static string Format(string source)
	{
		var tokens = new Scanner().Scan(source);
		var expressions = new Parser().Parse(tokens);

		return Formatter.Format(expressions);
	}
}