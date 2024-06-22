using System.Text;

using Specter.Color;

namespace Specter.Terminal.UI;


public class Border
{
	public static Border DefaultASCII { get => new('|', '-', '+'); }
	public static Border DefaultUTF8 { get => new('│', '─', ' ')
	{
		TopLeft = '╭',
		TopRight = '╮',
		BottomLeft = '╰',
		BottomRight = '╯'
	}; }

	public static Border Default { get => Terminal.GetOutputEncoding() switch
	{
		ASCIIEncoding => DefaultASCII,
        UTF8Encoding => DefaultUTF8,
		_ => DefaultASCII
	}; }


	public ColorObject Color { get; set; }

	public char Top { get; set; }
	public char Bottom { get; set; }
	public char Left { get; set; }
	public char Right { get; set; }


	public char TopLeft { get; set; }
	public char TopRight { get; set; }
	public char BottomLeft { get; set; }
	public char BottomRight { get; set; }


	public Border(char vertical, char horizontal, char corner, ColorObject? color = null)
	{
		Color = color ?? ColorObject.Default;

		Left = Right = vertical;
		Top = Bottom = horizontal;
		TopLeft = TopRight = BottomLeft = BottomRight = corner;
	}


	public static bool HasEdge(int edges, Bounds.Edge edge)
		=> (edges & (int)edge) == (int)edge;


	public char GetBorderCharFromEdgeFlags(int edges)
	{
		if (HasEdge(edges, Bounds.Edge.TopLeft))
			return TopLeft;

		if (HasEdge(edges, Bounds.Edge.TopRight))
			return TopRight;

		if (HasEdge(edges, Bounds.Edge.BottomLeft))
			return BottomLeft;

		if (HasEdge(edges, Bounds.Edge.BottomRight))
			return BottomRight;



		if (HasEdge(edges, Bounds.Edge.Top))
			return Top;

		if (HasEdge(edges, Bounds.Edge.Left))
			return Left;

		if (HasEdge(edges, Bounds.Edge.Bottom))
			return Bottom;

		if (HasEdge(edges, Bounds.Edge.Right))
			return Right;
		
		return '\0';
	}
}