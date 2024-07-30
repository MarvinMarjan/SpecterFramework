using System;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;



/// <summary>
/// All types of alignment.
/// </summary>
[Flags]
public enum Alignment
{
	None = 0,

	CenterHorizontal = 0b_0000_0001,
	Top              = 0b_0000_0010,
	Bottom           = 0b_0000_0100,

	CenterVertical = 0b_0000_1000,
	Left           = 0b_0001_0000,
	Right          = 0b_0010_0000,

	Center = CenterHorizontal | CenterVertical,

	TopCenter    = Top | CenterHorizontal,
	BottomCenter = Bottom | CenterHorizontal,
	LeftCenter   = Left | CenterVertical,
	RightCenter  = Right | CenterHorizontal,

	TopLeft     = Top | Left,
	TopRight    = Top | Right,
	BottomLeft  = Bottom | Left,
	BottomRight = Bottom | Right
}


public static class AlignmentExtensions
{
	private static uint CalculateCentralizedValue(uint parent, uint child)
	{
		double diff = Math.Abs((int)parent - (int)child);
		double position = diff / 2.0;
		
		bool hasRemainder = diff % 2 != 0;

		return (uint)(hasRemainder ? position + 1 : position);
	}


	/// <summary>
	/// Calculates the alignment.
	/// 
	/// * Note: Alignment may not be precisely, since terminal drawing units are rows and columns.
	/// </summary>
	/// <param name="alignment"> The alignments to use. </param>
	/// <param name="parent"> The parent Rect. </param>
	/// <param name="child"> The child Rect. </param>
	/// <returns> The aligned position. </returns>
	public static Point CalculatePosition(this Alignment alignment, Rect parent, Rect child)
	{
		Point finalPosition = child.Position;
		Size finalSize = child.Size;

		bool hasCenterH = alignment.HasFlag(Alignment.CenterHorizontal);
		bool hasCenterV = alignment.HasFlag(Alignment.CenterVertical);

		if (hasCenterH)
			finalPosition.Col = CalculateCentralizedValue(parent.Size.Width, child.Size.Width);

		if (hasCenterV)
			finalPosition.Row = CalculateCentralizedValue(parent.Size.Height, child.Size.Height);


		if (!hasCenterV)
		{
			if (alignment.HasFlag(Alignment.Top))
				finalPosition.Row = 0;

			else if (alignment.HasFlag(Alignment.Bottom))
				finalPosition.Row = parent.Size.Height - finalSize.Height;
		}
			

		if (!hasCenterH)
		{
			if (alignment.HasFlag(Alignment.Left))
				finalPosition.Col = 0;
	
			else if (alignment.HasFlag(Alignment.Right))
				finalPosition.Col = parent.Size.Width - finalSize.Width;
		}


		return finalPosition;
	}


	public static Point CalculatePosition(this Alignment alignment, Component parent, Component component)
		=> CalculatePosition(alignment, parent.Rect, component.Rect);
	


	public static Point CalculatePosition(this Alignment alignment, Component component)
	{
		if (component.Parent is null)
			return Point.None;

		return CalculatePosition(alignment, component.Parent.Rect, component.Rect);
	}
}