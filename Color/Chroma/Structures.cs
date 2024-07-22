namespace Specter.Color.Chroma;


public interface IStructure
{}

public interface INotationConvertableStructure : IStructure
{
	string ToNotation();
	bool IsDefaultNotation() => ToNotation() == "_";


	static INotationConvertableStructure DefaultNotation => new IdentifierStructure("_");
}


public class IdentifierStructure(string source) : INotationConvertableStructure, IExpressionConvertable
{
	public string Source { get; set; } = source;

	public string ToNotation() => Source;
	public IExpression ToExpression() => new TextExpression(Source);
}


public class RGBStructure(byte r, byte g, byte b) : INotationConvertableStructure
{
	public byte R { get; set; } = r;
	public byte G { get; set; } = g;
	public byte B { get; set; } = b;


	public string ToNotation()
		=> $"{R} {G} {B}";


	public static RGBStructure FromString(string source)
	{
		RGBStructure rgb = new(0, 0, 0);

		

		return rgb;
	}
}


public class FormatTagStructure(
	INotationConvertableStructure? fg,
	INotationConvertableStructure? bg,
	INotationConvertableStructure? mode
) : IStructure, IExpressionConvertable
{
	public INotationConvertableStructure Foreground { get; set; } = fg ?? INotationConvertableStructure.DefaultNotation;
	public INotationConvertableStructure Background { get; set; } = bg ?? INotationConvertableStructure.DefaultNotation;
	public INotationConvertableStructure Mode { get; set; } = mode ?? INotationConvertableStructure.DefaultNotation;

	public bool ResetTag { get; set; }


	public FormatTagStructure() : this(null, null, null)
	{
		ResetTag = true;
	}


	public ColorObject ToColorObject()
	{
		if (ResetTag)
			return ColorValue.Reset;

		ColorObject color = ColorObject.None;

		color.Foreground = Foreground.IsDefaultNotation() ? null : Notation.ToColorElement(Foreground.ToNotation(), ColorLayer.Foreground);
		color.Background = Background.IsDefaultNotation() ? null : Notation.ToColorElement(Background.ToNotation(), ColorLayer.Background);
		color.Mode       = Mode.IsDefaultNotation()       ? null : ColorTable.GetMode(Mode.ToNotation());

		return color;
	}


	public IExpression ToExpression()
		=> new FormatExpression(ToColorObject());
}