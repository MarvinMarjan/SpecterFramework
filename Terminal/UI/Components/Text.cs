using System.Collections.Generic;
using System.Text;

using Specter.ANSI;


namespace Specter.Terminal.UI.Components;


public class TextComponent : Component
{
    public InheritableComponentProperty<string> Text { get; set; }


    public TextComponent(Component? parent, Point? position = null, string? text = null) : base(parent, position)
    {
        Text = text ?? string.Empty;

		Properties.AddRange([ Text ]);
    }


    public override string Draw()
	{
		StringBuilder builder = new();

		builder.Append(ControlCodes.CursorTo(RelativePosition.row, RelativePosition.col));
		builder.Append(Color.Value.AsSequence());

		builder.Append(Text);

		builder.Append(EscapeCodes.Reset);

		return builder.ToString();
	}
}
