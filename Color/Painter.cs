using Specter.ANSI;

namespace Specter.Color;


public class Painter
{
	public static string Paint(string source, ColorObject? color = null)
	{
		color ??= new();

		return color.AsSequence() + source + EscapeCodes.Reset;
	}
}