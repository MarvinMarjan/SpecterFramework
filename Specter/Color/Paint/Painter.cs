using Specter.ANSI;


namespace Specter.Color.Paint;


/// <summary>
/// Provides a method to paint a string.
/// </summary>
public abstract class Painter
{
	/// <summary>
	/// The string placed at the end of a sequence.
	/// </summary>
	public string SequenceFinisher { get; set; } = EscapeCodes.Reset;


	/// <param name="source"> The string to be painted. </param>
	/// <returns> A painted string. </returns>
	public abstract string Paint(string source);


	// some pre-defined painting methods

	public static string Paint(string source, Painter painter) => painter.Paint(source);
	public static string Paint(string source, ColorObject color) => Paint(source, new ColorPainter(color));
	public static string Paint(string source, ColorPattern pattern) => Paint(source, new PatternPainter(pattern));
}