using System.Linq;

using Specter.ANSI;


namespace Specter.Color;


/// <summary>
/// Represents a sequence with both foreground, background and color mode of a text.
/// </summary>
/// <param name="fg"> The foreground element. </param>
/// <param name="bg"> The background element. </param>
/// <param name="mode"> The color mode. </param>
public class ColorObject(IANSISequenceElement? fg, IANSISequenceElement? bg, ColorMode? mode)
{
	public static ColorObject None { get => new(null, null, null); }



	public IANSISequenceElement? Foreground { get; set; } = fg;
	public IANSISequenceElement? Background { get; set; } = bg;
	public ColorMode? Mode { get; set; } = mode;



	/// <returns> A Color16-initialized ColorObject. </returns>
	/// <param name="fg"> The foreground color code. </param>
	/// <param name="bg"> The background color code. </param>
	/// <param name="mode"> The color mode. </param>
	public static ColorObject FromColor16(Color16? fg = null, Color16? bg = null, ColorMode? mode = null)
	{
		return new(new ColorCodeElement(fg), new ColorCodeElement(bg), mode);
	}


	/// <returns> A Color256-initialized ColorObject. </returns>
	/// <param name="fg"> The foreground color code. </param>
	/// <param name="bg"> The background color code. </param>
	/// <param name="mode"> The color mode. </param>
	public static ColorObject FromColor256(byte? fg = null, byte? bg = null, ColorMode? mode = null)
	{
		return new(new Color256Element(fg), new Color256Element(bg, ColorLayer.Background), mode);
	}


	/// <returns> A ColorRGB-initialized ColorObject. </returns>
	/// <param name="fg"> The foreground color code. </param>
	/// <param name="bg"> The background color code. </param>
	/// <param name="mode"> The color mode. </param>
	public static ColorObject FromColorRGB(ColorRGB? fg = null, ColorRGB? bg = null, ColorMode? mode = null)
	{
		return new(new ColorRGBElement(fg), new ColorRGBElement(bg, ColorLayer.Background), mode);
	}



	/// <returns> A ColorMode-initialized ColorObject. </returns>
	/// <param name="mode"> The color mode. </param>
	public static ColorObject FromColorMode(ColorMode? mode = null)
	{
		return new(null, null, mode);
	}


	/// <returns> An array containing a sequence of Color256-based ColorObjects. </returns>
	/// <param name="from"> The initial value. </param>
	/// <param name="to"> The final value. </param>
	public static ColorObject[] ArrayFromColor256Sequence(byte from, byte to)
	{
		var numberSequence = Enumerable.Range(from, to - from + 1);

		return (from number in numberSequence select FromColor256((byte)number)).ToArray();
	}


	// Use the ColorValue static members if Color16 is needed.

    public static implicit operator ColorObject(byte fg)        => FromColor256(fg);    // To Color256.
    public static implicit operator ColorObject(ColorRGB fg)    => FromColorRGB(fg);    // To ColorRGB.
    public static implicit operator ColorObject(ColorMode mode) => FromColorMode(mode); // To ColorMode.





    /// <summary>
	/// Try to merge two ColorObject properties. The Left-sided ColorObject have a higher priority.
	/// </summary>
	/// <param name="left"> The left-sided ColorObject. </param>
	/// <param name="right"> The right-sided ColorObject. </param>
	/// <returns> A new ColorObject with the properties merged. </returns>
    public static ColorObject operator+(ColorObject left, ColorObject right)
	{
		ColorObject result = new(
			fg: IANSISequenceElement.IsValid(left.Foreground) ? left.Foreground : right.Foreground,
			bg: IANSISequenceElement.IsValid(left.Background) ? left.Background : right.Background,
			mode: left.Mode ?? right.Mode
		);

		return result;
	}



	/// <returns> This ColorObject converted into an ANSI sequence. </returns>
	public string AsSequence()
		=> SequenceBuilder.BuildANSIEscapeSequence([ 
			((int?)Mode)?.ToString(), Foreground?.BuildSequence(), Background?.BuildSequence(),
		]);
}
