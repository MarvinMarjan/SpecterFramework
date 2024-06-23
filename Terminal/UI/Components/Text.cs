using System.Text;

using Specter.ANSI;
using Specter.Color;


namespace Specter.Terminal.UI.Components;


public class TextComponent : Component
{
    public InheritableComponentProperty<string> Text { get; set; }


    public TextComponent(
		Component? parent,
		Point? position = null,
		string text = "",
		ColorObject? color = null

	) : base(parent, position, color)
    {
        Text = text;

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
