using System;
using System.Text;
using Specter.ANSI;


namespace Specter.Terminal.UI.Components;


public class TextComponent(Component? parent, Point? position = null, string? text = null)
	: Component(parent, position)
{
	public string Text { get; set; } = text ?? string.Empty;



	public override string Draw()
	{
		StringBuilder builder = new();

		builder.Append(ControlCodes.CursorTo(RelativePosition.row, RelativePosition.col));
		builder.Append(Color.AsSequence());

		builder.Append(Text);

		builder.Append(EscapeCodes.Reset);

		return builder.ToString();
	}
}
