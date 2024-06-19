using System.Linq;
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
		if (pattern is null)
			return string.Empty;

		StringBuilder builder = new();
		ColorPattern validPattern = pattern ?? new();

		var colors = validPattern.colors;

		uint currentLength = 1;

		for (int charIndex = 0, colorIndex = 0; charIndex < source.Length; charIndex++)
		{
			// restart color index when it reaches the colors size
			if (colorIndex >= colors.Count)
				colorIndex = 0;

			var color = colors[colorIndex];
			char ch = source[charIndex];

			if (color.length == 0 || validPattern.ignoreChars.Contains(ch))
			{
				builder.Append(ch);
				continue;
			}

			// appends the painted character
			builder.Append(color.obj.AsSequence() + ch);

			if (currentLength++ < color.length)
				continue;

			currentLength = 1;
			colorIndex++;
		}

		builder.Append(EscapeCodes.Reset);

		return builder.ToString();
	}
}