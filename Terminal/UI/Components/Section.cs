using System;
using System.Collections.Immutable;
using System.Text;
using Specter.ANSI;
using Specter.Color;


namespace Specter.Terminal.UI.Components;


public class SectionComponent(Component? parent, Point? position = null, Size? size = null)
	: Component(parent, position)
{
	public Size Size { get; set; } = size ?? Size.None;
	
	public char BackgroundFill { get; set; } = ' ';


	public override string Draw()
	{
		StringBuilder builder = new();

		builder.Append(ControlCodes.CursorTo(RelativePosition.row, RelativePosition.col));
		builder.Append(Color.AsSequence());

		for (int i = 0; i < Size.height; i++)
		{
			for (int o = 0; o < Size.width; o++)
				builder.Append(BackgroundFill);

			builder.Append(ControlCodes.CursorDown(1) + ControlCodes.CursorToColumn(RelativePosition.col));
		}

		builder.Append(EscapeCodes.Reset);

		// draw childs
		builder.Append(base.Draw());

		return builder.ToString();
	}
}
