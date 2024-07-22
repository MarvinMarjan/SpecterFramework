using System.Linq;

using Specter.ANSI;


namespace Specter.Color.Chroma;


public static class Notation
{
	public static bool IsRGBNotation(string str, out byte[]? values)
	{
		string[] rgbChannels = str.Split(' ');

		int[] rgbValues = (from channel in rgbChannels
							where int.TryParse(channel, out int result)
							select int.Parse(channel)).ToArray();

		bool result = rgbValues.Length == 3 && rgbValues.All(value => value >= 0 && value <= 255);
	
		if (result)
			values = (from value in rgbValues select (byte)value).ToArray();
		else
			values = null;

		return result;
	}


	public static IANSISequenceElement? ToColorElement(string str, ColorLayer layer = ColorLayer.Foreground)
	{
		if (str.All(char.IsDigit))
		{
			bool success = byte.TryParse(str, out byte number);
			return success ? new Color256Element(number) : null;
		}

		else if (str.All(char.IsLetterOrDigit))
		{
			bool success = ColorTable.TryGetColor(str,layer, out ColorCodeElement? element);
			return success ? element : null;
		}

		else if (IsRGBNotation(str, out byte[]? values))
			return new ColorRGBElement(new(values?[0], values?[1], values?[2]));
		

		return null;
	}
}