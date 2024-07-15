using Specter.Color;


namespace Specter.String;


public static class StringFormat
{
	public static ColorObject DefaultQuoteColor { get; set; } = ColorValue.FGGreen;


	public static string Quote(this string source, ColorObject? color)
		=> (color is null ? ColorObject.None : color).Paint($@"""{source}""");

	
	public static string Quote(this string source)
		=> Quote(source, DefaultQuoteColor);
}
