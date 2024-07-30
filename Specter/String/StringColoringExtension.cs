using Specter.ANSI;
using Specter.Color;
using Specter.Color.Paint;


namespace Specter.String;


/// <summary>
/// Defines painting extension methods for the String object.
/// </summary>
public static class StringColoringExtension
{
	public static string Paint(this string str, Painter painter)
		=> Painter.Paint(str, painter);

	
	public static string Paint(this string str, ColorObject color) => Painter.Paint(str, color);
	public static string Paint(this string str, ColorPattern pattern) => Painter.Paint(str, pattern);


	public static string FGRed(this string str) => str.Paint(ColorValue.FGRed);
	public static string FGGreen(this string str) => str.Paint(ColorValue.FGGreen);
	public static string FGYellow(this string str) => str.Paint(ColorValue.FGYellow);
	public static string FGBlue(this string str) => str.Paint(ColorValue.FGBlue);
	public static string FGMagenta(this string str) => str.Paint(ColorValue.FGMagenta);
	public static string FGCyan(this string str) => str.Paint(ColorValue.FGCyan);
	public static string FGWhite(this string str) => str.Paint(ColorValue.FGWhite);
	public static string FGBlack(this string str) => str.Paint(ColorValue.FGBlack);

	public static string BGRed(this string str) => str.Paint(ColorValue.BGRed);
	public static string BGGreen(this string str) => str.Paint(ColorValue.BGGreen);
	public static string BGYellow(this string str) => str.Paint(ColorValue.BGYellow);
	public static string BGBlue(this string str) => str.Paint(ColorValue.BGBlue);
	public static string BGMagenta(this string str) => str.Paint(ColorValue.BGMagenta);
	public static string BGCyan(this string str) => str.Paint(ColorValue.BGCyan);
	public static string BGWhite(this string str) => str.Paint(ColorValue.BGWhite);
	public static string BGBlack(this string str) => str.Paint(ColorValue.BGBlack);


	public static string FGBRed(this string str) => str.Paint(ColorValue.FGBRed);
	public static string FGBGreen(this string str) => str.Paint(ColorValue.FGBGreen);
	public static string FGBYellow(this string str) => str.Paint(ColorValue.FGBYellow);
	public static string FGBBlue(this string str) => str.Paint(ColorValue.FGBBlue);
	public static string FGBMagenta(this string str) => str.Paint(ColorValue.FGBMagenta);
	public static string FGBCyan(this string str) => str.Paint(ColorValue.FGBCyan);
	public static string FGBWhite(this string str) => str.Paint(ColorValue.FGBWhite);
	public static string FGBBlack(this string str) => str.Paint(ColorValue.FGBBlack);

	public static string BGBRed(this string str) => str.Paint(ColorValue.BGBRed);
	public static string BGBGreen(this string str) => str.Paint(ColorValue.BGBGreen);
	public static string BGBYellow(this string str) => str.Paint(ColorValue.BGBYellow);
	public static string BGBBlue(this string str) => str.Paint(ColorValue.BGBBlue);
	public static string BGBMagenta(this string str) => str.Paint(ColorValue.BGBMagenta);
	public static string BGBCyan(this string str) => str.Paint(ColorValue.BGBCyan);
	public static string BGBWhite(this string str) => str.Paint(ColorValue.BGBWhite);
	public static string BGBBlack(this string str) => str.Paint(ColorValue.BGBBlack);
}
