using System.Security.Cryptography;

namespace Specter.Color;


public static class ColorValue
{
	public static ColorObject FGBlack { get; } = new(foreground: new ColorCodeElement(Color16.FGBlack));
	public static ColorObject FGRed { get; } = new(foreground: new ColorCodeElement(Color16.FGRed));
	public static ColorObject FGGreen { get; } = new(foreground: new ColorCodeElement(Color16.FGGreen));
	public static ColorObject FGYellow { get; } = new(foreground: new ColorCodeElement(Color16.FGYellow));
	public static ColorObject FGBlue { get; } = new(foreground: new ColorCodeElement(Color16.FGBlue));
	public static ColorObject FGMagenta { get; } = new(foreground: new ColorCodeElement(Color16.FGMagenta));
	public static ColorObject FGCyan { get; } = new(foreground: new ColorCodeElement(Color16.FGCyan));
	public static ColorObject FGWhite { get; } = new(foreground: new ColorCodeElement(Color16.FGWhite));
	public static ColorObject FGDefault { get; } = new(foreground: new ColorCodeElement(Color16.FGDefault));


	public static ColorObject BGBlack { get; } = new(background: new ColorCodeElement(Color16.BGBlack));
	public static ColorObject BGRed { get; } = new(background: new ColorCodeElement(Color16.BGRed));
	public static ColorObject BGGreen { get; } = new(background: new ColorCodeElement(Color16.BGGreen));
	public static ColorObject BGYellow { get; } = new(background: new ColorCodeElement(Color16.BGYellow));
	public static ColorObject BGBlue { get; } = new(background: new ColorCodeElement(Color16.BGBlue));
	public static ColorObject BGMagenta { get; } = new(background: new ColorCodeElement(Color16.BGMagenta));
	public static ColorObject BGCyan { get; } = new(background: new ColorCodeElement(Color16.BGCyan));
	public static ColorObject BGWhite { get; } = new(background: new ColorCodeElement(Color16.BGWhite));
	public static ColorObject BGDefault { get; } = new(background: new ColorCodeElement(Color16.BGDefault));


	public static ColorObject FGBBlack { get; } = new(foreground: new ColorCodeElement(Color16.FGBBlack));
	public static ColorObject FGBRed { get; } = new(foreground: new ColorCodeElement(Color16.FGBRed));
	public static ColorObject FGBGreen { get; } = new(foreground: new ColorCodeElement(Color16.FGBGreen));
	public static ColorObject FGBYellow { get; } = new(foreground: new ColorCodeElement(Color16.FGBYellow));
	public static ColorObject FGBBlue { get; } = new(foreground: new ColorCodeElement(Color16.FGBBlue));
	public static ColorObject FGBMagenta { get; } = new(foreground: new ColorCodeElement(Color16.FGBMagenta));
	public static ColorObject FGBCyan { get; } = new(foreground: new ColorCodeElement(Color16.FGBCyan));
	public static ColorObject FGBWhite { get; } = new(foreground: new ColorCodeElement(Color16.FGBWhite));


	public static ColorObject BGBBlack { get; } = new(background: new ColorCodeElement(Color16.BGBBlack));
	public static ColorObject BGBRed { get; } = new(background: new ColorCodeElement(Color16.BGBRed));
	public static ColorObject BGBGreen { get; } = new(background: new ColorCodeElement(Color16.BGBGreen));
	public static ColorObject BGBYellow { get; } = new(background: new ColorCodeElement(Color16.BGBYellow));
	public static ColorObject BGBBlue { get; } = new(background: new ColorCodeElement(Color16.BGBBlue));
	public static ColorObject BGBMagenta { get; } = new(background: new ColorCodeElement(Color16.BGBMagenta));
	public static ColorObject BGBCyan { get; } = new(background: new ColorCodeElement(Color16.BGBCyan));
	public static ColorObject BGBWhite { get; } = new(background: new ColorCodeElement(Color16.BGBWhite));


	public static ColorObject Normal { get; } = new (mode: ColorMode.Normal);
	public static ColorObject Bold { get; } = new (mode: ColorMode.Bold);
	public static ColorObject Dim { get; } = new (mode: ColorMode.Dim);
	public static ColorObject Italic { get; } = new (mode: ColorMode.Italic);
	public static ColorObject Underline { get; } = new (mode: ColorMode.Underline);
	public static ColorObject Blinking { get; } = new (mode: ColorMode.Blinking);
	public static ColorObject Inverse { get; } = new (mode: ColorMode.Inverse);
	public static ColorObject Hidden { get; } = new (mode: ColorMode.Hidden);
	public static ColorObject Strike { get; } = new (mode: ColorMode.Strike);
}