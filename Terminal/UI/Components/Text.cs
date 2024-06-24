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

		Alignment? alignment = null,
		
		ColorObject? color = null,

		string text = ""


	) : base(parent, position, null, alignment, color) // * size is set in Update()
    {
        Text = text;

		Properties.AddRange([ Text ]);
    }


	protected Size SizeFromText() => new((uint)Text.Value.Length, 1);


    public override string Draw()
	{
		StringBuilder builder = new();

		builder.Append(ControlCodes.CursorTo(RelativePosition.row, RelativePosition.col));
		builder.Append(Color.Value.AsSequence());

		builder.Append(Text);

		builder.Append(EscapeCodes.Reset);


		builder.Append(base.Draw());

		return builder.ToString();
	}

    public override void Update()
    {
		Size.Value = SizeFromText();

        base.Update();
    }
}
