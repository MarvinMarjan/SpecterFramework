using System;
using System.Collections.Generic;


namespace Specter.Color.Chroma;


public static class ColorTable
{
	private readonly static Dictionary<string, ColorCodeElement> s_colorTable = new([
		new("fgblack", Color16.FGBlack),
		new("fgred", Color16.FGRed),
		new("fggreen", Color16.FGGreen),
		new("fgyellow", Color16.FGYellow),
		new("fgblue", Color16.FGBlue),
		new("fgmagenta", Color16.FGMagenta),
		new("fgcyan", Color16.FGCyan),
		new("fgwhite", Color16.FGWhite),
		new("fgdefault", Color16.FGDefault),

		new("fgbblack", Color16.FGBBlack),
		new("fgbred", Color16.FGBRed),
		new("fgbgreen", Color16.FGBGreen),
		new("fgbyellow", Color16.FGBYellow),
		new("fgbblue", Color16.FGBBlue),
		new("fgbmagenta", Color16.FGBMagenta),
		new("fgbcyan", Color16.FGBCyan),
		new("fgbwhite", Color16.FGBWhite),


		new("bgblack", Color16.BGBlack),
		new("bgred", Color16.BGRed),
		new("bggreen", Color16.BGGreen),
		new("bgyellow", Color16.BGYellow),
		new("bgblue", Color16.BGBlue),
		new("bgmagenta", Color16.BGMagenta),
		new("bgcyan", Color16.BGCyan),
		new("bgwhite", Color16.BGWhite),
		new("bgdefault", Color16.BGDefault),

		new("bgbblack", Color16.BGBBlack),
		new("bgbred", Color16.BGBRed),
		new("bgbgreen", Color16.BGBGreen),
		new("bgbyellow", Color16.BGBYellow),
		new("bgbblue", Color16.BGBBlue),
		new("bgbmagenta", Color16.BGBMagenta),
		new("bgbcyan", Color16.BGBCyan),
		new("bgbwhite", Color16.BGBWhite)
	]);


	private readonly static Dictionary<string, ColorMode> s_colorModeTable = new([
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


	/// <summary>
	/// Gets a color in the table.
	/// </summary>
	/// <param name="colorName"> The name of the color. </param>
	/// <param name="layer"> The color layer. </param>
	/// <returns> The searched color as a color element. </returns>
	/// <exception cref="ChromaException"> Exception thrown if it was not possible to find the color. </exception>
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
			throw new ChromaException(null, $"Invalid color name: {colorName}");

		return element ?? Color16.Reset;
	}

	/// <summary>
	/// Same as "ColorTable.GetColor", but ignores exceptions.
	/// </summary>
	/// <param name="colorName"> The color name. </param>
	/// <param name="layer"> The color layer. </param>
	/// <param name="colorElement"> The out variable to store the value. </param>
	/// <returns> True if the operation succeeded, false otherwise. </returns>
	public static bool TryGetColor(string colorName, ColorLayer layer, out ColorCodeElement? colorElement)
	{
		try
		{
			colorElement = GetColor(colorName, layer);
			return true;
		}
		catch (Exception)
		{
			colorElement = null;
			return false;
		}
	}



	/// <summary>
	/// Gets a color mode in the table.
	/// </summary>
	/// <param name="colorModeName"> The color mode name. </param>
	/// <returns> The searched color mode. </returns>
	/// <exception cref="ChromaException"> Exception thrown if it was not possible to find the color mode. </exception>
	public static ColorMode GetMode(string colorModeName)
	{
		bool found = s_colorModeTable.TryGetValue(colorModeName, out ColorMode mode);
	
		if (!found)
			throw new ChromaException(null, $"Invalid color mode name: {colorModeName}");

		return mode;
	}

	/// <summary>
	/// Same as "ColorTable.GetMode", but ignores exceptions.
	/// </summary>
	/// <param name="colorModeName"> The color mode name. </param>
	/// <param name="mode"> The out variable to store de color mode. </param>
	/// <returns> True if the operation succeeded, false otherwise. </returns>
	public static bool TryGetMode(string colorModeName, out ColorMode? mode)
	{
		try
		{
			mode = GetMode(colorModeName);
			return true;
		}
		catch (Exception)
		{
			mode = null;
			return false;
		}
	}
}