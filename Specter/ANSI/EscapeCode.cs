namespace Specter.ANSI;


/// <summary>
/// Some useful ANSI escape codes definition
/// </summary>
public static class EscapeCodes
{
	public const string Octal = "\033";
	public const string Unicode = "\u001b";
	public const string Hexadecimal = "\x1b";

	public const string DefaultEscapeCode = Hexadecimal;
	public const string EscapeCodeWithController = DefaultEscapeCode + "[";

	public const char EscapeCodeEnd = 'm';

	public const string Reset = DefaultEscapeCode + "[" + "0m";

	public const char Color256TypeCode = '5';
	public const char ColorRGBTypeCode = '2';
}