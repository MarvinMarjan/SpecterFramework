using System.Text;

namespace Specter.ANSI;



// Interface for representing a ANSI sequence element.
// At the sequence "\x1b[0;31;45m", the numbers "0", "31" and "45" are
// elements.
public interface IANSISequenceElement
{
	// Checks whether a sequence element is valid or not.
	public static bool IsValid(IANSISequenceElement? element) => element is not null && element.IsValid();


	// Checks whether the current object has a valid sequence or not.
	public bool IsValid();
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
		
		builder.Append(string.Join(';', validCodes));

		if (useEscapeCode)
			builder.Append('m');
		
		return builder.ToString();
	}
}
