using System.Collections.Generic;


namespace Specter.Color.Chroma;


public static class ColorTable
{
	private static Dictionary<string, ColorCodeElement> s_colorTable = new([
		new("fgblack", Color16.FGBlack),
		new("fgred", Color16.FGRed),
		new("fgyellow", Color16.FGYellow),
		new("fgblue", Color16.FGBlue),
		new("fgmagenta", Color16.FGMagenta),
		new("fgcyan", Color16.FGCyan),
		new("fgwhite", Color16.FGWhite),
		new("fgdefault", Color16.FGDefault),

		new("fgbblack", Color16.FGBBlack),
		new("fgbred", Color16.FGBRed),
		new("fgbyellow", Color16.FGBYellow),
		new("fgbblue", Color16.FGBBlue),
		new("fgbmagenta", Color16.FGBMagenta),
		new("fgbcyan", Color16.FGBCyan),
		new("fgbwhite", Color16.FGBWhite),


		new("bgblack", Color16.BGBlack),
		new("bgred", Color16.BGRed),
		new("bgyellow", Color16.BGYellow),
		new("bgblue", Color16.BGBlue),
		new("bgmagenta", Color16.BGMagenta),
		new("bgcyan", Color16.BGCyan),
		new("bgwhite", Color16.BGWhite),
		new("bgdefault", Color16.BGDefault),

		new("bgbblack", Color16.BGBBlack),
		new("bgbred", Color16.BGBRed),
		new("bgbyellow", Color16.BGBYellow),
		new("bgbblue", Color16.BGBBlue),
		new("bgbmagenta", Color16.BGBMagenta),
		new("bgbcyan", Color16.BGBCyan),
		new("bgbwhite", Color16.BGBWhite)
	]);


	private static Dictionary<string, ColorMode> s_colorModeTable = new([
		new("normal", ColorMode.Normal),
		new("bold", ColorMode.Bold),
		new("dim", ColorMode.Dim),
		new("italic", ColorMode.Italic),
		new("underline", ColorMode.Underline),
		new("blinking", ColorMode.Blinking),
		new("inverse", ColorMode.Inverse),
		new("hidden", ColorMode.Hidden),
		new("strike", ColorMode.Strike)
	]);


	public static ColorCodeElement GetColor(string colorName, ColorLayer layer)
	{
		string finalColorName = layer switch
		{
			ColorLayer.Foreground => "fg",
			ColorLayer.Background => "bg",

			_ => ""
		} + colorName;

		bool found = s_colorTable.TryGetValue(finalColorName, out ColorCodeElement? element);
	
		if (!found)
			throw new ChromaException($"Invalid color name: {colorName}");

		return element ?? Color16.Reset;
	}


	public static ColorMode GetMode(string colorModeName)
	{
		bool found = s_colorModeTable.TryGetValue(colorModeName, out ColorMode mode);
	
		if (!found)
			throw new ChromaException($"Invalid color mode name: {colorModeName}");


		return mode;
	}
}