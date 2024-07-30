namespace Specter.Color.Paint;


/// <summary>
/// A Painter for ColorObjects.
/// </summary>
/// <param name="color"> The ColorObject to use. </param>
public class ColorPainter(ColorObject? color = null) : Painter
{
	public ColorObject? Color { get; set; } = color;


	public override string Paint(string source)
	{
		if (Color is null)
			return string.Empty;

		return Color.AsSequence() + source + SequenceFinisher;
	}
}