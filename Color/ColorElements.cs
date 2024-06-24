using Specter.ANSI;


namespace Specter.Color;


/// <summary>
/// The layer of a color. Color elements like Color256 or ColorRGB
/// should use this enum to define which layer they represents.
/// </summary>
public enum ColorLayer
{
	Foreground = 38,
	Background = 48
}


/// <summary>
/// A color element for Color16.
/// </summary>
/// <param name="code"> The color code. </param>
public class ColorCodeElement(Color16? code = null) : IANSISequenceElement
{
	public Color16? code = code;


	public bool IsValid() => code is not null;

    public string BuildSequence()
	{
		int? intCode = (int?)code;

		return intCode?.ToString() ?? string.Empty;
	}
}


/// <summary>
/// A color element for 8-bit (0-255) coloring.
/// 
/// Take a look at https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797#colors--graphics-mode
/// </summary>
/// <param name="code"> The 8-bit color code. </param>
/// <param name="layer"> The layer of the element. </param>
public class Color256Element(byte? code = null, ColorLayer layer = ColorLayer.Foreground) : IANSISequenceElement
{
	public byte? code = code;
	public ColorLayer layer = layer;


	public bool IsValid() => code is not null;

	public string BuildSequence()
	{
		if (!IsValid())
			return string.Empty;

		int layerCode = (int)layer;

		return SequenceBuilder.BuildANSIEscapeSequence([
			layerCode.ToString(), EscapeCodes.Color256TypeCode, code.ToString()
		], false);
	}
}


/// <summary>
/// A color element for RGB based coloring.
/// </summary>
/// <param name="color"> The RGB color. </param>
/// <param name="layer"> The layer of the element. </param>
public class ColorRGBElement(ColorRGB? color = null, ColorLayer layer = ColorLayer.Foreground) : IANSISequenceElement
{
	public ColorRGB? color = color;
	public ColorLayer layer = layer;


	public bool IsValid() => color is not null;

	public string BuildSequence()
	{
		if (!IsValid())
			return string.Empty;

		int layerCode = (int)layer;

		var validColor = color ?? new ColorRGB();

		if (!validColor.AreAllChannelsNull())
			validColor.SetValueToNullChannels(0);

		return SequenceBuilder.BuildANSIEscapeSequence([
			layerCode.ToString(), EscapeCodes.ColorRGBTypeCode,
			validColor.r?.ToString(), validColor.g?.ToString(), validColor.b?.ToString()
		], false);
	}
}