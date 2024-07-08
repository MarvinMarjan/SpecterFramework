using System.Text;

using Specter.ANSI;
using Specter.Color;


namespace Specter.Terminal.UI.Components;


/// <summary>
/// Represents a text component.
/// * Note: Can't be defined as parent of another Component.
/// </summary>
public class TextComponent : Component, IChildLess
{
	new protected object[] Properties => [ Text ];


    public ComponentProperty<string> Text { get; }


    public TextComponent(

		Component? parent,
		Point? position = null,

		Alignment? alignment = null,
		
		ColorObject? color = null,

		bool inheritProperties = false,

		string text = ""


		// * size is set in Update()
	) : base(parent, position, null, alignment, color, inheritProperties)
    {
        Text = text;

		AllProperties.AddRange(Properties);

		Properties.PropertiesAs<IInheritable>().SetInheritablesInherit(inheritProperties);
		RequestRenderOnPropertyChange(Text, true);
    }


	/// <returns> A Size object based on Text. </returns>
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
		Size.DefaultValue = SizeFromText();

        base.Update();
    }
}
