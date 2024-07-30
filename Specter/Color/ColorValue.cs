namespace Specter.Color;


/// <summary>
/// Pre-defined ColorObjects for all Color16 codes.
/// </summary>
public static class ColorValue
{
	public static ColorObject Reset { get => ColorObject.FromColor16(fg: Color16.Reset); }


	public static ColorObject FGBlack { get => ColorObject.FromColor16(fg: Color16.FGBlack); }
	public static ColorObject FGRed { get => ColorObject.FromColor16(fg: Color16.FGRed); }
	public static ColorObject FGGreen { get => ColorObject.FromColor16(fg: Color16.FGGreen); }
	public static ColorObject FGYellow { get => ColorObject.FromColor16(fg: Color16.FGYellow); }
	public static ColorObject FGBlue { get => ColorObject.FromColor16(fg: Color16.FGBlue); }
	public static ColorObject FGMagenta { get => ColorObject.FromColor16(fg: Color16.FGMagenta); }
	public static ColorObject FGCyan { get => ColorObject.FromColor16(fg: Color16.FGCyan); }
	public static ColorObject FGWhite { get => ColorObject.FromColor16(fg: Color16.FGWhite); }
	public static ColorObject FGDefault { get => ColorObject.FromColor16(fg: Color16.FGDefault); }


	public static ColorObject BGBlack { get => ColorObject.FromColor16(bg: Color16.BGBlack); }
	public static ColorObject BGRed { get => ColorObject.FromColor16(bg: Color16.BGRed); }
	public static ColorObject BGGreen { get => ColorObject.FromColor16(bg: Color16.BGGreen); }
	public static ColorObject BGYellow { get => ColorObject.FromColor16(bg: Color16.BGYellow); }
	public static ColorObject BGBlue { get => ColorObject.FromColor16(bg: Color16.BGBlue); }
	public static ColorObject BGMagenta { get => ColorObject.FromColor16(bg: Color16.BGMagenta); }
	public static ColorObject BGCyan { get => ColorObject.FromColor16(bg: Color16.BGCyan); }
	public static ColorObject BGWhite { get => ColorObject.FromColor16(bg: Color16.BGWhite); }
	public static ColorObject BGDefault { get => ColorObject.FromColor16(bg: Color16.BGDefault); }


	public static ColorObject FGBBlack { get => ColorObject.FromColor16(fg: Color16.FGBBlack); }
	public static ColorObject FGBRed { get => ColorObject.FromColor16(fg: Color16.FGBRed); }
	public static ColorObject FGBGreen { get => ColorObject.FromColor16(fg: Color16.FGBGreen); }
	public static ColorObject FGBYellow { get => ColorObject.FromColor16(fg: Color16.FGBYellow); }
	public static ColorObject FGBBlue { get => ColorObject.FromColor16(fg: Color16.FGBBlue); }
	public static ColorObject FGBMagenta { get => ColorObject.FromColor16(fg: Color16.FGBMagenta); }
	public static ColorObject FGBCyan { get => ColorObject.FromColor16(fg: Color16.FGBCyan); }
	public static ColorObject FGBWhite { get => ColorObject.FromColor16(fg: Color16.FGBWhite); }


	public static ColorObject BGBBlack { get => ColorObject.FromColor16(bg: Color16.BGBBlack); }
	public static ColorObject BGBRed { get => ColorObject.FromColor16(bg: Color16.BGBRed); }
	public static ColorObject BGBGreen { get => ColorObject.FromColor16(bg: Color16.BGBGreen); }
	public static ColorObject BGBYellow { get => ColorObject.FromColor16(bg: Color16.BGBYellow); }
	public static ColorObject BGBBlue { get => ColorObject.FromColor16(bg: Color16.BGBBlue); }
	public static ColorObject BGBMagenta { get => ColorObject.FromColor16(bg: Color16.BGBMagenta); }
	public static ColorObject BGBCyan { get => ColorObject.FromColor16(bg: Color16.BGBCyan); }
	public static ColorObject BGBWhite { get => ColorObject.FromColor16(bg: Color16.BGBWhite); }


	public static ColorObject Normal { get => ColorObject.FromColorMode(ColorMode.Normal); }
	public static ColorObject Bold { get => ColorObject.FromColorMode(ColorMode.Bold); }
	public static ColorObject Dim { get => ColorObject.FromColorMode(ColorMode.Dim); }
	public static ColorObject Italic { get => ColorObject.FromColorMode(ColorMode.Italic); }
	public static ColorObject Underline { get => ColorObject.FromColorMode(ColorMode.Underline); }
	public static ColorObject Blinking { get => ColorObject.FromColorMode(ColorMode.Blinking); }
	public static ColorObject Inverse { get => ColorObject.FromColorMode(ColorMode.Inverse); }
	public static ColorObject Hidden { get => ColorObject.FromColorMode(ColorMode.Hidden); }
	public static ColorObject Strike { get => ColorObject.FromColorMode(ColorMode.Strike); }
}