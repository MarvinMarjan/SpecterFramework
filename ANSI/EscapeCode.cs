namespace Specter.ANSI;


public static class EscapeCodes
{
	public const string Octal = "\033";
	public const string Unicode = "\u001b";
	public const string Hexadecimal = "\x1b";

	public const string DefaultEscapeCode = Hexadecimal;
	public const string EscapeCodeWithController = DefaultEscapeCode + "[";

	public const string Reset = DefaultEscapeCode + "[" + "0m";

	public const string Color256TypeCode = "5";
	public const string ColorRGBTypeCode = "2";
}