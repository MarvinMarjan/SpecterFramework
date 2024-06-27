using System.Linq;
using System.Text;

using Specter.ANSI;


namespace Specter.Color;


/// <summary>
/// Provides a method to paint a string.
/// </summary>
public abstract class Painter
{
	/// <summary>
	/// The string placed at the end of a sequence.
	/// </summary>
	public string SequenceFinisher { get; set; } = EscapeCodes.Reset;


	/// <param name="source"> The string to be painted. </param>
	/// <returns> A painted string. </returns>
	public abstract string Paint(string source);


	// some pre-defined painting methods

	public static string Paint(string source, Painter painter) => painter.Paint(source);
	public static string Paint(string source, ColorObject color) => Paint(source, new ColorPainter(color));
	public static string Paint(string source, ColorPattern pattern) => Paint(source, new PatternPainter(pattern));
}


/// <summary>
/// A Painter for ColorObjects.
/// </summary>
/// <param name="color"> The ColorObject to use. </param>
public class ColorPainter(ColorObject? color = null) : Painter
{
	public ColorObject? Color { get; set; } = color;


	public override string Paint(string source)
	{
		if (Color is null)
			return string.Empty;

		return Color.AsSequence() + source + SequenceFinisher;
	}
}

/// <summary>
/// A Painter for ColorPatterns.
/// </summary>
/// <param name="pattern"> The pattern to be used. </param>
public class PatternPainter(ColorPattern? pattern = null) : Painter
{
	public ColorPattern? Pattern { get; set; } = pattern;


	public override string Paint(string source)
	{
		if (Pattern is null)
			return string.Empty;

		StringBuilder builder = new();
		ColorPattern validPattern = Pattern ?? new();

		var colors = validPattern.Colors;
		uint currentLength = 1;

		for (int charIndex = 0, colorIndex = 0; charIndex < source.Length; charIndex++)
		{
			// TODO: split these into separated methods

			// restart color index when it reaches the colors size
			if (colorIndex >= colors.Count)
			{
				if (validPattern.ResetMode == ResetMode.Revert)
					colors.Reverse();
				
				colorIndex = 0;
			}

			var color = colors[colorIndex];
			char ch = source[charIndex];

			if (color.length == 0 || validPattern.IgnoreChars.Contains(ch))
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

		builder.Append(SequenceFinisher);

		return builder.ToString();
	}
}