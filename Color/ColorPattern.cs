using System.Collections.Generic;
using System.Linq;


namespace Specter.Color;


public struct ColorPattern(List<ColorPattern.Color> colors, char[]? ignoreChars = null)
{
	public struct Color(ColorObject obj, uint length)
	{
		public ColorObject obj = obj;
		public uint length = length;
	}
	

	public List<Color> colors = colors;
	public char[] ignoreChars = ignoreChars ?? [];


	public ColorPattern(List<ColorObject> colors, uint colorLength = 1, char[]? ignoreChars = null)
		: this([], ignoreChars)
	{
		this.colors = (from color in colors select new Color(color, colorLength)).ToList();
	}
}
