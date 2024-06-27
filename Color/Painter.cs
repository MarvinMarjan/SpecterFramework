using System.Collections.Generic;
using System.ComponentModel;
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
	public ColorPattern ValidPattern => Pattern ?? new ColorPattern();
	public List<ColorPattern.Color> Colors => ValidPattern.Colors;
	public ColorPattern.Color CurrentColor => Colors[(int)_currentLength];


	private uint _charIndex;
	private uint _colorIndex;
	private uint _currentLength;


	// Restart color index when it reaches the colors size.
	private void ResetColorIndex()
	{
		if (ValidPattern.ResetMode == ResetMode.Revert)
			Colors.Reverse();
		
		_colorIndex = 0;
	}

	private bool ShouldIgnore(char ch)
		=> CurrentColor.length == 0 || ValidPattern.IgnoreChars.Contains(ch);

	private bool ColorLengthReached()
		=> _currentLength++ >= CurrentColor.length;

	private void NextColor()
	{
		_currentLength = 1;
		_colorIndex++;
	}


	public override string Paint(string source)
	{
		if (Pattern is null)
			return string.Empty;

		StringBuilder builder = new();
		_currentLength = 1;

		for (_charIndex = 0, _colorIndex = 0; _charIndex < source.Length; _charIndex++)
		{
			if (_colorIndex >= Colors.Count)
				ResetColorIndex();

			char ch = source[(int)_charIndex];

			if (ShouldIgnore(ch))
			{
				builder.Append(ch);
				continue;
			}

			// appends the painted character
			builder.Append(CurrentColor.obj.AsSequence() + ch);

			if (ColorLengthReached())
				NextColor();
		}

		builder.Append(SequenceFinisher);

		return builder.ToString();
	}
}