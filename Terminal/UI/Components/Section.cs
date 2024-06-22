using System.Text;

using Specter.ANSI;
using Specter.Color;


namespace Specter.Terminal.UI.Components;


public class SectionComponent : Component
{

	public Size Size { get; set; }
	public Bounds Bounds { get => Bounds.FromRectangle(Position, Size); }
	public Border Border { get; set; }
	public bool DrawBorder { get; set; }
	
	public char BackgroundFill { get; set; }


	public SectionComponent(Component? parent, Point? position = null, Size? size = null)
		: base(parent, position)
	{
		Size = size ?? Size.None;

		Border = Border.Default;
		Border.Color = Color;

		DrawBorder = true;

		BackgroundFill = ' ';
	}


	protected void DrawAt(ref StringBuilder builder, uint row, uint col)
	{
		// draws the border
		if (DrawBorder && Bounds.IsAtBorder(new Point(row, col) + Position, out int edges))
		{
			ColorPainter painter = new(Border.Color) { SequenceFinisher = Color.AsSequence() };

			builder.Append(painter.Paint(new(Border.GetBorderCharFromEdgeFlags(edges), 1)));
			return;
		}

		// draws the background
		builder.Append(BackgroundFill);
	}


	public override string Draw()
	{
		StringBuilder builder = new();

		builder.Append(ControlCodes.CursorTo(RelativePosition.row, RelativePosition.col));
		builder.Append(Color.AsSequence());

		for (int i = 0; i < Size.height; i++)
		{
			for (int o = 0; o < Size.width; o++)
				DrawAt(ref builder, (uint)i, (uint)o);

			builder.Append(ControlCodes.CursorDown(1) + ControlCodes.CursorToColumn(RelativePosition.col));
		}

		builder.Append(EscapeCodes.Reset);

		// draw childs
		builder.Append(base.Draw());

		return builder.ToString();
	}
}
