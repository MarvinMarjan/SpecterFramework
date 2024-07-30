using System;
using System.Text;


namespace Specter.Terminal.UI;


/// <summary>
/// Represents each character of a border.
/// </summary>
public struct BorderCharacters
{
	public static BorderCharacters UTF8Default => new(new UTF8Encoding());
	public static BorderCharacters ASCIIDefault => new(new ASCIIEncoding());

	public static BorderCharacters Default => Console.OutputEncoding switch 
	{
		UTF8Encoding => UTF8Default,
		ASCIIEncoding => ASCIIDefault,
		_ => ASCIIDefault
	};



	public char Top { get; set; }
	public char Bottom { get; set; }
	public char Left { get; set; }
	public char Right { get; set; }


	public char TopLeft { get; set; }
	public char TopRight { get; set; }
	public char BottomLeft { get; set; }
	public char BottomRight { get; set; }


	public BorderCharacters(Encoding encoding) => SetBorderCharacters(encoding);


	public void SetBorderCharacters(char vertical, char horizontal, char corner)
	{
		Left = Right = vertical;
		Top = Bottom = horizontal;
		TopLeft = TopRight = BottomLeft = BottomRight = corner;
	}


	/// <summary>
	/// Initialization based on the encoding.
	/// </summary>
	/// <param name="encoding"> The encoding. </param>
	public void SetBorderCharacters(Encoding encoding)
	{
		switch (encoding)
		{
			case ASCIIEncoding:
				SetBorderCharacters('|', '-', '+');
				break;

			case UnicodeEncoding:
			case UTF8Encoding:
				SetBorderCharacters('│', '─', ' ');
				TopLeft = '╭';
				TopRight = '╮';
				BottomLeft = '╰';
				BottomRight = '╯';
				break;

			default:
				SetBorderCharacters(new ASCIIEncoding());
				break;
		}
	}


	public readonly char GetBorderCharFromEdgeFlags(Bounds.Edge edges)
	{
		if (Bounds.HasEdgeInEdges(edges, Bounds.Edge.TopLeft))
			return TopLeft;

		if (Bounds.HasEdgeInEdges(edges, Bounds.Edge.TopRight))
			return TopRight;

		if (Bounds.HasEdgeInEdges(edges, Bounds.Edge.BottomLeft))
			return BottomLeft;

		if (Bounds.HasEdgeInEdges(edges, Bounds.Edge.BottomRight))
			return BottomRight;



		if (Bounds.HasEdgeInEdges(edges, Bounds.Edge.Top))
			return Top;

		if (Bounds.HasEdgeInEdges(edges, Bounds.Edge.Left))
			return Left;

		if (Bounds.HasEdgeInEdges(edges, Bounds.Edge.Bottom))
			return Bottom;

		if (Bounds.HasEdgeInEdges(edges, Bounds.Edge.Right))
			return Right;
		
		return '\0';
	}
}