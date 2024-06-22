﻿using System.Linq;

using Specter.ANSI;


namespace Specter.Color;


public class ColorObject(IANSISequenceElement? fg, IANSISequenceElement? bg, ColorMode? mode)
{
	public static ColorObject None { get => new(null, null, null); }

	public IANSISequenceElement? foreground = fg;
	public IANSISequenceElement? background = bg;
	public ColorMode? mode = mode;



	public static ColorObject FromColor16(Color16? fg = null, Color16? bg = null, ColorMode? mode = null)
	{
		return new(new ColorCodeElement(fg), new ColorCodeElement(bg), mode);
	}

	public static ColorObject FromColor256(byte? fg = null, byte? bg = null, ColorMode? mode = null)
	{
		return new(new Color256Element(fg), new Color256Element(bg, ColorLayer.Background), mode);
	}

	public static ColorObject FromColorRGB(ColorRGB? fg = null, ColorRGB? bg = null, ColorMode? mode = null)
	{
		return new(new ColorRGBElement(fg), new ColorRGBElement(bg, ColorLayer.Background), mode);
	}

	public static ColorObject FromColorMode(ColorMode? mode = null)
	{
		return new(null, null, mode);
	}


	public static ColorObject[] ArrayFromColor256Sequence(byte from, byte to)
	{
		var numberSequence = Enumerable.Range(from, to - from + 1);

		return (from number in numberSequence select FromColor256((byte)number)).ToArray();
	}


	// use ColorValue fields if a Color16 object is required

    public static implicit operator ColorObject(byte fg) => FromColor256(fg);
    public static implicit operator ColorObject(ColorRGB fg) => FromColorRGB(fg);
    public static implicit operator ColorObject(ColorMode mode) => FromColorMode(mode);





    // creates a new ColorObject where a field of it has the value of "right"
    // if "left"'s field is null.
    public static ColorObject operator+(ColorObject left, ColorObject right)
	{
		ColorObject result = new(
			fg: IANSISequenceElement.IsValid(left.foreground) ? left.foreground : right.foreground,
			bg: IANSISequenceElement.IsValid(left.background) ? left.background : right.background,
			mode: left.mode ?? right.mode
		);

		return result;
	}


	public string AsSequence()
	{
		return SequenceBuilder.BuildANSIEscapeSequence([ 
			((int?)mode)?.ToString(), foreground?.BuildSequence(), background?.BuildSequence(),
		]);
	}
}
