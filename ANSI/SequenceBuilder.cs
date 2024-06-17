using System.Text;

namespace Specter.ANSI;



// Interface for representing a ANSI sequence element.
// At the sequence "\x1b[0;31;45m", the numbers "0", "31" and "45" are
// elements.
public interface IANSISequenceElement
{
	public string BuildSequence();
}


public static class SequenceBuilder
{
	// Builds an ANSI sequence based on the codes in "codes".
	// i.e: the input ["1", "34", "102"] generates "\x1b[1;34;102m"
	public static string BuildANSIEscapeSequence(string?[] codes, bool useEscapeCode = true)
	{
		StringBuilder builder = new();

		if (useEscapeCode)
			builder.Append(EscapeCodes.Esc + "[");

		// removes any null or empty value from "codes"
		var validCodes = (from code in codes where code is not null and not "" select code).ToArray();

		for (int i = 0; i < validCodes.Length; i++)
		{
			bool atEnd = i + 1 >= validCodes.Length;
			string code = validCodes[i];

			builder.Append(code + (!atEnd ? ";" : ""));
		}

		if (useEscapeCode)
			builder.Append('m');
		
		return builder.ToString();
	}
}
