using System.Collections.Generic;
using System.Linq;


namespace Specter.Color;


public struct ColorPattern(List<ColorPattern.Color> colors)
{
	public struct Color(ColorObject obj, uint length)
	{
		public ColorObject obj = obj;
		public uint length = length;
	}


	public enum ResetMode
	{
		FromBeginning,
		Revert
	}


	public List<Color> colors = colors;
	public char[] ignoreChars = [];
	public ResetMode resetMode = ResetMode.FromBeginning;


	public ColorPattern(List<ColorObject> colors, uint colorLength = 1)
		: this([])
	{
		this.colors = (from color in colors select new Color(color, colorLength)).ToList();
	}
}
