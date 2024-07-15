namespace Specter.Color.Paint;


/// <summary>
/// Create extension methods for the String object.
/// </summary>
public static class PaintExtensions
{
	public static string Paint(this ColorObject color, string source)
		=> Painter.Paint(source, color);


	public static string Paint(this ColorPattern pattern, string source)
		=> Painter.Paint(source, pattern);
}
