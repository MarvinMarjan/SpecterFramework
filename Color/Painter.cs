using System.Collections.Generic;
using System.Text;

using Specter.ANSI;


namespace Specter.Color;


public interface IPainter
{
	public string Paint(string source);

	public static string Paint(string source, IPainter painter) => painter.Paint(source);


	public static string Paint(string source, ColorObject color) => Paint(source, new ColorPainter(color));
	public static string Paint(string source, ColorPattern pattern) => Paint(source, new PatternPainter(pattern));
}


public class ColorPainter(ColorObject? color = null) : IPainter
{
	public ColorObject? color = color;


	public string Paint(string source)
	{
		if (color is null)
			return string.Empty;

		return color.AsSequence() + source + EscapeCodes.Reset;
	}
}


public class PatternPainter(ColorPattern? pattern = null) : IPainter
{
	public ColorPattern? pattern = pattern;


	public string Paint(string source)
	{
		if (pattern is null || pattern?.length is 0)
			return string.Empty;

		StringBuilder builder = new();
		ColorPattern validPattern = pattern ?? new();

		List<ColorObject> colors = validPattern.colors;


		for (int charIndex = 0, colorIndex = 0; charIndex < source.Length; charIndex++, colorIndex++)
		{
			if (colorIndex == colors.Count)
				colorIndex = 0;

			ColorObject color = colors[colorIndex];

			builder.Append(color.AsSequence() + source[charIndex]);
		}

		builder.Append(EscapeCodes.Reset);

		return builder.ToString();
	}
}