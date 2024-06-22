namespace Specter.Terminal.UI;



public struct Point(uint row, uint col)
{
	public static Point None { get => new (1, 1); }

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
}


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
}


public struct Bounds(uint top, uint left, uint bottom, uint right)
{
    public uint top = top;
    public uint left = left;
    public uint bottom = bottom;
    public uint right = right;


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


    public readonly bool IsAtBorder(Point point)
        => (point.row == top || point.row == bottom) && point.col >= left && point.col <= right ||
			(point.col == left || point.col == right) && point.row >= top && point.row <= bottom;


	public readonly bool IsAtBorder(Point point, out int edges)
	{
		edges = 0;

		if (!IsAtBorder(point))
		{
			edges = 0;
			return false;
		}

		edges |= point.row == top ? (int)Edge.Top : 0;
		edges |= point.row == bottom ? (int)Edge.Bottom : 0;
		edges |= point.col == left ? (int)Edge.Left : 0;
		edges |= point.col == right ? (int)Edge.Right : 0;

		return true;
	}
}