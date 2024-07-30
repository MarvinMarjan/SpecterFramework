using System.Linq;

using Specter.ANSI;


namespace Specter.Color.Chroma;


// Notation is a string representation of a color in Chroma.
// color codes are represented by names: "fgred", "bggreen", etc...
// 8-bit color codes are represented by 0-255 ids: "116", "231".
// RGB colors are represented by the following notation: "(redChannel, greenChannel, blueChannel)"
// like "(255, 100, 210)"


public static class Notation
{
	/// <summary>
	/// Checks if the specified string is a valid RGB notation. If it is,
	/// then the RGB channels are stored into a out-parameter.
	/// </summary>
	/// <param name="str"> The string to verify. </param>
	/// <param name="values"> The out-parameter to store the channels. </param>
	/// <returns> True if the operation succeeded, false otherwise. </returns>
	public static bool IsRGBNotation(string str, out byte[]? values)
	{
		string[] rgbChannels = str.Split(' ');

		// store the channels as numbers.
		int[] rgbValues = (from channel in rgbChannels
							where int.TryParse(channel, out int result)
							select int.Parse(channel)).ToArray();

		bool result = rgbValues.Length == 3 && rgbValues.All(value => value >= 0 && value <= 255);
	
		if (result)
			values = (from value in rgbValues select (byte)value).ToArray(); // convert to byte numbers
		else
			values = null;

		return result;
	}


	/// <summary>
	/// Converts a notation to a color element.
	/// </summary>
	/// <param name="str"> The notation to convert. </param>
	/// <param name="layer"> The color layer. </param>
	/// <returns> A color element created from a notation. </returns>
	public static IANSISequenceElement? ToColorElement(string str, ColorLayer layer = ColorLayer.Foreground)
	{
		// 8-bit colors
		if (str.All(char.IsDigit))
		{
			bool success = byte.TryParse(str, out byte number);
			return success ? new Color256Element(number) : null;
		}

		// color codes
		else if (str.All(char.IsLetter))
		{
			bool success = ColorTable.TryGetColor(str,layer, out ColorCodeElement? element);
			return success ? element : null;
		}

		// RGB
		else if (IsRGBNotation(str, out byte[]? values))
			return new ColorRGBElement(new(values?[0], values?[1], values?[2]));

		// invalid
		return null;
	}

	public static IANSISequenceElement? ToColorElement(INotationConvertableStructure structure, ColorLayer layer = ColorLayer.Foreground)
		=> ToColorElement(structure.ToNotation(), layer);
}