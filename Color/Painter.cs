using Specter.ANSI;

namespace Specter.Color;

public class Painter
{
	public static string Paint(string source)
	{
		return SequenceBuilder.BuildANSIEscapeSequence(new string[] {"1", "90", "255"});
	}
}