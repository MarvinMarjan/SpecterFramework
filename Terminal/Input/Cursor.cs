using Specter.Color;
using Specter.Color.Paint;
using Specter.Terminal.UI;

namespace Specter.Terminal.Input;


/// <summary>
/// Wraps some useful cursor data.
/// </summary>
/// <param name="color"> The color of the cursor. </param>
public class Cursor(ColorObject? color = null)
{
	public ColorObject Color { get; set; } = color ?? ColorValue.Inverse;
	public ColorObject EndColor { get; set; } = ColorValue.Reset;
	

	private int _index = 0;
	public int Index
	{
		get => _index;
		set
		{
			if (value <= IndexLimit && value >= 0)
				_index = value;
		}
	}
	
	public int ValidIndex => Index >= IndexLimit && Index - 1 >= 0 ? Index - 1 : Index;

	public int IndexLimit { get; set; }
	public Point Position { get; set; }


	public string GetCursorAtEnd() => Color.Paint("_");

	/// <summary>
	/// Draws this cursor to a string.
	///
	/// * Warning: Don't use this to draw the cursor directly on a already painted string, since the result may
	/// * be buggy. For that purpose, you can use RulePainter and attach a Cursor to it. "RulePainter.Cursor"
	/// </summary>
	/// <param name="source"> The string to be drawn. </param>
	/// <param name="index"> The optional index. </param>
	/// <param name="endColor"> The optional color to be used after the cursor. </param>
	/// <returns> The source string with the cursor drawn. </returns>
	public string DrawTo(string source, int? index = null, ColorObject? endColor = null)
	{
		string result = source;

		int finalIndex = index ?? Index;
		int finalLimit = index is null ? IndexLimit : source.Length;

		ColorObject oldEndColor = EndColor;
		EndColor = endColor ?? EndColor;

		if (finalIndex >= finalLimit)
			result += GetCursorAtEnd();
		
		else
		{
			result = result.Insert(finalIndex + 1, ColorValue.Reset.AsSequence() + EndColor.AsSequence());
			result = result.Insert(finalIndex, Color.AsSequence());
		}

		EndColor = oldEndColor;

		return result;
	}

	/// <summary>
	/// Draws the cursor to a single character.
	/// </summary>
	/// <param name="ch"> The character to be drawn. </param>
	/// <param name="endColor"> The color to use after the cursor. </param>
	/// <returns> A string with the cursor drawed at the character. </returns>
	public string DrawTo(char ch, ColorObject? endColor = null)
		=> DrawTo(ch.ToString(), 0, endColor);
}
