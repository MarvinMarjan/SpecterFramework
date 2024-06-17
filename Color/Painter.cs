using Specter.ANSI;

namespace Specter.Color;


public class Painter
{
	public static string Paint(string source, ColorObject? color = null)
	{
		color ??= ColorObject.Default;

		return color.AsSequence() + source + EscapeCodes.Reset;
	}
}