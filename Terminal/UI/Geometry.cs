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