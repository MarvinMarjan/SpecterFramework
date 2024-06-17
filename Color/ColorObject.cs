using Specter.ANSI;

namespace Specter.Color;


public class ColorObject(IANSISequenceElement? foreground = null, IANSISequenceElement? background = null, ColorMode? mode = null)
{
	public static ColorObject Default { get; } = new ColorObject();

	public IANSISequenceElement? foreground = foreground;
	public IANSISequenceElement? background = background;
	public ColorMode? mode = mode;


	// creates a new ColorObject where a field of it has the value of "right"
	// if "left"'s field is null.
	public static ColorObject operator+(ColorObject left, ColorObject right)
	{
        ColorObject result = new() {
            foreground = left.foreground ?? right.foreground,
            background = left.background ?? right.background,
            mode = left.mode ?? right.mode
        };

        return result;
	}


    public string AsSequence()
	{
		return SequenceBuilder.BuildANSIEscapeSequence([ 
			((int?)mode)?.ToString(), foreground?.BuildSequence(), background?.BuildSequence(),
		]);
	}
}
