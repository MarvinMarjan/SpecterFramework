using Specter.Color;


namespace Specter.String;


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
}
