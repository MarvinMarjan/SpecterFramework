namespace Specter.Color.Chroma;


/// <summary>
/// Represent Chroma structures.
/// </summary>
public interface IStructure
{}

/// <summary>
/// Represents a structure that can be converted to a notation representation.
/// </summary>
public interface INotationConvertableStructure : IStructure
{
	string ToNotation();
	bool IsDefaultNotation() => ToNotation() == "_";


	static INotationConvertableStructure DefaultNotation => new IdentifierStructure("_");
}


/// <summary>
/// Represents identifiers: "red", "78", etc...
/// </summary>
public class IdentifierStructure(string source) : INotationConvertableStructure, IExpressionConvertable
{
	public string Source { get; set; } = source;

	public string ToNotation() => Source;
	public IExpression ToExpression() => new TextExpression(Source);
}


/// <summary>
/// Represents a RGB color: "(255, 0, 23)"
/// </summary>
public class RGBStructure(byte r, byte g, byte b) : INotationConvertableStructure
{
	public byte R { get; set; } = r;
	public byte G { get; set; } = g;
	public byte B { get; set; } = b;


	public string ToNotation()
		=> $"{R} {G} {B}";
}


/// <summary>
/// Represents a format tag: "<green red underline>"
/// </summary>
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


	/// <summary>
	/// Converts this tag to a ColorObject.
	/// </summary>
	public ColorObject ToColorObject()
	{
		if (ResetTag)
			return ColorValue.Reset;

		ColorObject color = ColorObject.None;

		color.Foreground = Foreground.IsDefaultNotation() ? null : Notation.ToColorElement(Foreground, ColorLayer.Foreground);
		color.Background = Background.IsDefaultNotation() ? null : Notation.ToColorElement(Background, ColorLayer.Background);
		color.Mode       = Mode.IsDefaultNotation()       ? null : ColorTable.GetMode(Mode.ToNotation());

		return color;
	}


	public IExpression ToExpression()
		=> new FormatExpression(ToColorObject());
}