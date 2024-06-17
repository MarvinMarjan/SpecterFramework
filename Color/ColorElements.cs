using Specter.ANSI;

namespace Specter.Color;


public class ColorCodeElement(Color16? code = null) : IANSISequenceElement
{
	public Color16? code = code;

    public string BuildSequence()
	{
		int? intCode = (int?)code;

		return intCode?.ToString() ?? string.Empty;
	}
}


public class Color256Element(int? code = null, Color256Element.Layer layer = Color256Element.Layer.Foreground) : IANSISequenceElement
{
	public enum Layer
	{
		Foreground = 38,
		Background = 48
	}


	public int? code = code;
	public Layer layer = layer;

	public string BuildSequence()
	{
		int layerCode = (int)layer;

		return SequenceBuilder.BuildANSIEscapeSequence([
			layerCode.ToString(), EscapeCodes.Color256TypeCode, code?.ToString() ?? string.Empty
		], false);
	}
}