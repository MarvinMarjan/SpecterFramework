using System.Text;
using Specter.Color;
using Specter.Color.Paint;
using Specter.Terminal.UI;

namespace Specter.Terminal.Input;


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

	public string DrawTo(char ch, ColorObject? endColor = null)
		=> DrawTo(ch.ToString(), 0, endColor);
}
