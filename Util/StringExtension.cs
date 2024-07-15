using System.Linq;


namespace Specter.Util;


public static class StringExtensions
{
	public static bool IsAlpha(this string source)
		=> source.All(char.IsLetter);

	public static bool IsAlphaNumeric(this string source)
		=> source.All(char.IsLetterOrDigit);
}