using System.Text;

namespace Specter.ANSI;

public static class SequenceBuilder
{
	public static string BuildANSIEscapeSequence(string[] codes)
	{
		StringBuilder builder = new();

		builder.Append(EscapeCodes.Esc + "[");

		for (int i = 0; i < codes.Count(); i++)
		{
			bool atEnd = i + 1 >= codes.Length;
				builder.Append(codes[i] + (!atEnd ? ";" : ""));
		}

		builder.Append('m');
		
		return builder.ToString();
	}
}
