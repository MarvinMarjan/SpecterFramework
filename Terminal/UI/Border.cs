using Specter.Color;

namespace Specter.Terminal.UI;


public struct Border
{
	public static Border Default { get => new('|', '-', '+'); }

	public ColorObject color;

	public char top;
	public char bottom;
	public char left;
	public char right;


	public char topLeft;
	public char topRight;
	public char bottomLeft;
	public char bottomRight;


	public Border(char vertical, char horizontal, char corner, ColorObject? color = null)
	{
		this.color = color ?? ColorObject.Default;

		left = right = vertical;
		top = bottom = horizontal;
		topLeft = topRight = bottomLeft = bottomRight = corner;
	}


	public static bool HasEdge(int edges, Bounds.Edge edge)
		=> (edges & (int)edge) == (int)edge;


	public readonly char GetBorderCharFromEdgeFlags(int edges)
	{
		if (HasEdge(edges, Bounds.Edge.TopLeft))
			return topLeft;

		if (HasEdge(edges, Bounds.Edge.TopRight))
			return topRight;

		if (HasEdge(edges, Bounds.Edge.BottomLeft))
			return bottomLeft;

		if (HasEdge(edges, Bounds.Edge.BottomRight))
			return bottomRight;



		if (HasEdge(edges, Bounds.Edge.Top))
			return top;

		if (HasEdge(edges, Bounds.Edge.Left))
			return left;

		if (HasEdge(edges, Bounds.Edge.Bottom))
			return bottom;

		if (HasEdge(edges, Bounds.Edge.Right))
			return right;
		
		return '\0';
	}
}