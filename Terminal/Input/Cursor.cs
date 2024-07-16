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


	public string DrawTo(string source)
	{
		string result = source;

		if (Index >= IndexLimit)
			result += Color.Paint("_");
		
		else
		{
			result = result.Insert(Index + 1, EndColor.AsSequence());
			result = result.Insert(Index, Color.AsSequence());
		}

		return result;
	}
}
