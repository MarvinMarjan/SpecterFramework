using System.Text;
using System.Linq;
using System.Collections.Generic;


namespace Specter.Color.Paint;


/// <summary>
/// A Painter for ColorPatterns.
/// </summary>
/// <param name="pattern"> The pattern to be used. </param>
public class PatternPainter(ColorPattern? pattern = null) : Painter
{
	public ColorPattern? Pattern { get; set; } = pattern;
	public ColorPattern ValidPattern => Pattern ?? new ColorPattern();
	public List<ColorPattern.Color> Colors => ValidPattern.Colors;
	public ColorPattern.Color CurrentColor => Colors[(int)_colorIndex];


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