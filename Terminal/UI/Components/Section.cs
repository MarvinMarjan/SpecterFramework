using System.Text;

using Specter.ANSI;
using Specter.Color;


namespace Specter.Terminal.UI.Components;


public class SectionComponent : Component
{
	// Properties

	public Bounds Bounds { get => Bounds.FromRectangle(Position, Size); }


	// Component properties

	public ComponentProperty<Size> Size { get; }
	
	public InheritableComponentProperty<char> BackgroundFill { get; }
	public InheritableComponentProperty<BorderCharacters> BorderCharacters { get; }
	public InheritableComponentProperty<ColorObject> BorderColor { get; }
	public InheritableComponentProperty<bool> DrawBorder { get; }


	public SectionComponent(
		Component? parent,
		Point? position = null,
		Size? size = null,
	
		ColorObject? color = null,

		BorderCharacters? borderCharacters = null,
		ColorObject? borderColor = null,
		bool drawBorder = true,
		char backgroundFill = ' '
	
	) : base(parent, position, color)
	{
		Size = size ?? UI.Size.None;


		BorderCharacters = new(
			borderCharacters ?? UI.BorderCharacters.Default, Parent?.As<SectionComponent>()?.BorderCharacters
		);

		BorderColor = new(
			borderColor ?? Color ?? ColorValue.Reset, Parent?.As<SectionComponent>()?.BorderColor
		);

		DrawBorder = new(
			drawBorder, Parent?.As<SectionComponent>()?.DrawBorder
		);

		BackgroundFill = new(
			backgroundFill, Parent?.As<SectionComponent>()?.BackgroundFill
		);


		Properties.AddRange([ Size, BorderCharacters, BorderColor, DrawBorder, BackgroundFill ]);
	}


	protected void DrawAt(ref StringBuilder builder, uint row, uint col)
	{
		// draws the border
		if (DrawBorder && Bounds.IsAtBorder(new Point(row, col) + Position, out int edges))
		{
			ColorPainter painter = new(BorderColor) { SequenceFinisher = Color.Value.AsSequence() };

			builder.Append(painter.Paint(new(BorderCharacters.Value.GetBorderCharFromEdgeFlags(edges), 1)));
			return;
		}

		// draws the background
		builder.Append(BackgroundFill);
	}


	public override string Draw()
	{
		StringBuilder builder = new();

		builder.Append(ControlCodes.CursorTo(RelativePosition.row, RelativePosition.col));
		builder.Append(Color.Value.AsSequence());

		for (int i = 0; i < Size.Value.height; i++)
		{
			for (int o = 0; o < Size.Value.width; o++)
				DrawAt(ref builder, (uint)i, (uint)o);

			builder.Append(ControlCodes.CursorDown(1) + ControlCodes.CursorToColumn(RelativePosition.col));
		}

		builder.Append(EscapeCodes.Reset);

		// draw childs
		builder.Append(base.Draw());

		return builder.ToString();
	}
}
