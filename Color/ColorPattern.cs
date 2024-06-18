using System.Collections.Generic;


namespace Specter.Color;


public struct ColorPattern(List<ColorObject> colors, uint length = 1)
{
	public List<ColorObject> colors = colors;
	public uint length = length;
}
