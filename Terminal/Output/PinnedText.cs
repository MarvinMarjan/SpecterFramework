using System.Text;

using Specter.Terminal.UI;
using Specter.Terminal.UI.Components;


namespace Specter.Terminal.Output;


public class PinnedText(string text, Point position) : IDrawable
{
	private string _text = text;
	public string Text
	{
		get => _text;
		set
		{
			if (_text.Length > _lastTextHigherSize)
				_lastTextHigherSize = _text.Length;
			
			_text = value;
		}
	}
	
	private int _lastTextHigherSize;

	public Point Position { get; set; } = position;

	public bool EraseOnDraw { get; set; }


	public static PinnedText FromCurrent(bool newLine = true)
	{
		PinnedText text = new("", TerminalAttributes.CursorPosition);
	
		if (newLine)
			TerminalStream.WriteLine();

		return text;
	}


	public void Write(string text)
	{
		Text = text;
		TerminalStream.Write(Draw());
	}


	public string Draw()
	{
		StringBuilder builder = new();

		builder.Append(ControlCodes.CursorTo(Position.row, Position.col));

		if (EraseOnDraw)
		{
			builder.Append(GetEraseString());
			builder.Append(ControlCodes.CursorTo(Position.row, Position.col));
		}

		builder.Append(Text);

		return builder.ToString();
	}


	private string GetEraseString()
	{
		string text = new(' ', _lastTextHigherSize);

		_lastTextHigherSize = 0;
		return text;
	}
}