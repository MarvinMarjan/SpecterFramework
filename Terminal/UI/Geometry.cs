using System;
using System.Diagnostics.CodeAnalysis;

namespace Specter.Terminal.UI;


/// <summary>
/// Represents a point in the terminal.
/// </summary>
/// <param name="row"> The row position. </param>
/// <param name="col"> The column position. </param>
public struct Point(uint row, uint col)
{
	public static Point None { get => new (0, 0); }

	public uint row = row;
	public uint col = col;


	public static Point operator+(Point left, Point right)
		=> new(left.row + right.row, left.col + right.col);

	public static Point operator-(Point left, Point right)
		=> new(left.row - right.row, left.col - right.col);

	public static Point operator*(Point left, Point right)
		=> new(left.row * right.row, left.col * right.col);

	public static Point operator/(Point left, Point right)
		=> new(left.row / right.row, left.col / right.col);

	
	public static bool operator==(Point left, Point right)
		=> left.row == right.row && left.col == right.col;

	public static bool operator!=(Point left, Point right)
		=> left.row != right.row || left.col != right.col;


	public override readonly bool Equals(object? obj) => base.Equals(obj);
    public override readonly int GetHashCode() => base.GetHashCode();
}


/// <summary>
/// Represents a size in the terminal.
/// </summary>
/// <param name="width"> The width. </param>
/// <param name="height"> The height. </param>
public struct Size(uint width, uint height)
{
	public static Size None { get => new(0, 0); }

	public uint width = width;
	public uint height = height;


	public static Size operator+(Size left, Size right)
		=> new(left.width + right.width, left.height + right.height);

	public static Size operator-(Size left, Size right)
		=> new(left.width - right.width, left.height - right.height);

	public static Size operator*(Size left, Size right)
		=> new(left.width * right.width, left.height * right.height);

	public static Size operator/(Size left, Size right)
		=> new(left.width / right.width, left.height / right.height);

	
	public static bool operator==(Size left, Size right)
		=> left.width == right.width && left.height == right.height;

	public static bool operator!=(Size left, Size right)
		=> left.width != right.width || left.height != right.height;


    public override readonly bool Equals(object? obj) => base.Equals(obj);
    public override readonly int GetHashCode() => base.GetHashCode();
}


/// <summary>
/// Encapsulates both position and size in a single object.
/// </summary>
/// <param name="position"> The position. </param>
/// <param name="size"> The size. </param>
public struct Rect(Point position, Size size)
{
	public Point position = position;
	public Size size = size;
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
    public uint top = top;
    public uint left = left;
    public uint bottom = bottom;
    public uint right = right;


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
		=> new(position.row, position.col, position.row + size.height - 1, position.col + size.width - 1);


	public static bool HasEdgeInEdges(Edge edges, Edge edge)
		=> (edges & edge) == edge;


    public readonly bool IsAtBorder(Point point)
        => (point.row == top || point.row == bottom) && point.col >= left && point.col <= right ||
			(point.col == left || point.col == right) && point.row >= top && point.row <= bottom;


	public readonly bool IsAtBorder(Point point, out Edge edges)
	{
		edges = 0;

		if (!IsAtBorder(point))
		{
			edges = 0;
			return false;
		}

		edges |= point.row == top ? Edge.Top : 0;
		edges |= point.row == bottom ? Edge.Bottom : 0;
		edges |= point.col == left ? Edge.Left : 0;
		edges |= point.col == right ? Edge.Right : 0;

		return true;
	}
}