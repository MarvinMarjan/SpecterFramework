using System;


namespace Specter.Terminal.UI;


/// <summary>
/// Represents a point in the terminal.
/// </summary>
/// <param name="row"> The row position. </param>
/// <param name="col"> The column position. </param>
public struct Point(uint row, uint col)
{
	public static Point None { get => new (0, 0); }

	public uint Row { get; set; } = row;
	public uint Col { get; set; } = col;


	public static Point operator+(Point left, Point right)
		=> new(left.Row + right.Row, left.Col + right.Col);

	public static Point operator-(Point left, Point right)
		=> new(left.Row - right.Row, left.Col - right.Col);

	public static Point operator*(Point left, Point right)
		=> new(left.Row * right.Row, left.Col * right.Col);

	public static Point operator/(Point left, Point right)
		=> new(left.Row / right.Row, left.Col / right.Col);

	
	public static bool operator==(Point left, Point right)
		=> left.Equals(right);

	public static bool operator!=(Point left, Point right)
		=> !left.Equals(right);


	public readonly override int GetHashCode() => (Row, Col).GetHashCode();

	public readonly override bool Equals(object? obj)
		=> obj is Point point && Equals(point);

	public readonly bool Equals(Point obj) => Row == obj.Row && Col == obj.Col;


	public override readonly string ToString()
		=> $"Pos(row: {Row}, col: {Col})";
}


/// <summary>
/// Represents a size in the terminal.
/// </summary>
/// <param name="width"> The width. </param>
/// <param name="height"> The height. </param>
public struct Size(uint width, uint height) : IEquatable<Size>
{
	public static Size None { get => new(0, 0); }

	public uint Width { get; set; } = width;
	public uint Height { get; set; } = height;


	public static Size operator+(Size left, Size right)
		=> new(left.Width + right.Width, left.Height + right.Height);

	public static Size operator-(Size left, Size right)
		=> new(left.Width - right.Width, left.Height - right.Height);

	public static Size operator*(Size left, Size right)
		=> new(left.Width * right.Width, left.Height * right.Height);

	public static Size operator/(Size left, Size right)
		=> new(left.Width / right.Width, left.Height / right.Height);


	public static bool operator==(Size left, Size right)
		=> left.Equals(right);

	public static bool operator!=(Size left, Size right)
		=> !left.Equals(right);


	public readonly override int GetHashCode() => (Width, Height).GetHashCode();

	public readonly override bool Equals(object? obj)
		=> obj is Size size && Equals(size);

	public readonly bool Equals(Size obj) => Width == obj.Width && Height == obj.Height;


	public readonly override string ToString()
		=> $"Size(width: {Width}, height: {Height})";
}


/// <summary>
/// Encapsulates both position and size in a single object.
/// </summary>
/// <param name="position"> The position. </param>
/// <param name="size"> The size. </param>
public struct Rect(Point position, Size size)
{
	public Point Position { get; set; } = position;
	public Size Size { get; set; } = size;
}


/// <summary>
/// Represents the bounds of a rectangle in the terminal.
/// </summary>
/// <param name="top"> The top. </param>
/// <param name="left"> The left. </param>
/// <param name="bottom"> The bottom. </param>
/// <param name="right"> The right. </param>
public struct Bounds(uint top, uint left, uint bottom, uint right)
{
	public uint Top { get; set; } = top;
	public uint Left { get; set; } = left;
	public uint Bottom { get; set; } = bottom;
	public uint Right { get; set; } = right;


	[Flags]
	public enum Edge
	{
		None = 0b0000,

		Top = 0b0001,
		Left = 0b0010,
		Bottom = 0b0100,
		Right = 0b1000,

		TopLeft = Top | Left,
		TopRight = Top | Right,
		BottomLeft = Bottom | Left,
		BottomRight = Bottom | Right
	}


	public static Bounds FromRectangle(Point position, Size size)
		=> new(position.Row, position.Col, position.Row + size.Height - 1, position.Col + size.Width - 1);


	public static bool HasEdgeInEdges(Edge edges, Edge edge)
		=> (edges & edge) == edge;


	public readonly bool IsAtBorder(Point point)
		=> (point.Row == Top || point.Row == Bottom) && point.Col >= Left && point.Col <= Right ||
			(point.Col == Left || point.Col == Right) && point.Row >= Top && point.Row <= Bottom;


	public readonly bool IsAtBorder(Point point, out Edge edges)
	{
		edges = 0;

		if (!IsAtBorder(point))
		{
			edges = 0;
			return false;
		}

		edges |= point.Row == Top ? Edge.Top : 0;
		edges |= point.Row == Bottom ? Edge.Bottom : 0;
		edges |= point.Col == Left ? Edge.Left : 0;
		edges |= point.Col == Right ? Edge.Right : 0;

		return true;
	}
}