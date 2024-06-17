using Specter.ANSI;

namespace Specter.Color;


public enum ColorLayer
{
	Foreground = 38,
	Background = 48
}


public class ColorCodeElement(Color16? code = null) : IANSISequenceElement
{
	public Color16? code = code;

    public string BuildSequence()
	{
		int? intCode = (int?)code;

		return intCode?.ToString() ?? string.Empty;
	}
}


public class Color256Element(byte? code = null, ColorLayer layer = ColorLayer.Foreground) : IANSISequenceElement
{
	public byte? code = code;
	public ColorLayer layer = layer;

	public string BuildSequence()
	{
		int layerCode = (int)layer;

		return SequenceBuilder.BuildANSIEscapeSequence([
			layerCode.ToString(), EscapeCodes.Color256TypeCode, code?.ToString()
		], false);
	}
}


public class ColorRGBElement(RGBColor color = new(), ColorLayer layer = ColorLayer.Foreground) : IANSISequenceElement
{
	public RGBColor color = color;
	public ColorLayer layer = layer;


	public string BuildSequence()
	{
		int layerCode = (int)layer;

		if (!color.AreAllChannelsNull())
			color.SetValueToNullChannels(0);

		return SequenceBuilder.BuildANSIEscapeSequence([
			layerCode.ToString(), EscapeCodes.ColorRGBTypeCode,
			color.r?.ToString(), color.g?.ToString(), color.b?.ToString()
		], false);
	}
}