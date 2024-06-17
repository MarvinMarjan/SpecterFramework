using System.Reflection.Metadata.Ecma335;
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


	public bool IsValid() => code is not null;

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


	public bool IsValid() => code is not null;

	public string BuildSequence()
	{
		if (code is null)
			return string.Empty;

		int layerCode = (int)layer;

		return SequenceBuilder.BuildANSIEscapeSequence([
			layerCode.ToString(), EscapeCodes.Color256TypeCode, code.ToString()
		], false);
	}
}


public class ColorRGBElement(ColorRGB? color = null, ColorLayer layer = ColorLayer.Foreground) : IANSISequenceElement
{
	public ColorRGB? color = color;
	public ColorLayer layer = layer;


	public bool IsValid() => color is not null;

	public string BuildSequence()
	{
		if (color is null)
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