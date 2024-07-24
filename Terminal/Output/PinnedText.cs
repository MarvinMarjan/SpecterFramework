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


	public static PinnedText FromCurrent()
		=> new("", TerminalAttributes.CursorPosition);


	/// <summary>
	/// Creates a array of pinned texts. Each pinned text is on a line.
	/// </summary>
	/// <param name="count"> The amount of pinned texts to create. </param>
	/// <returns> The array of pinned texts. </returns>
	public static PinnedText[] CreateCount(int count)
	{
		// make sure the lines already exists
		TerminalStream.AllocateLines(count);

		PinnedText[] texts = new PinnedText[count];

		for (int i = 0; i < count; i++)
		{
			texts[i] = FromCurrent();
			TerminalStream.WriteLine();
		}

		return texts;
	}


	public void Write(string text)
	{
		Text = text;
		WriteToTerminal();
	}


	public void WriteToTerminal()
		=> TerminalStream.Write(Draw());


	public string Draw()
	{
		StringBuilder builder = new();

		builder.Append(ControlCodes.CursorTo(Position.Row, Position.Col));

		if (EraseOnDraw)
		{
			builder.Append(GetEraseString());
			builder.Append(ControlCodes.CursorTo(Position.Row, Position.Col));
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