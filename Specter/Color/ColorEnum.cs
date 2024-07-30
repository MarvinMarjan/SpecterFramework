namespace Specter.Color;


/// <summary>
/// All the 16 (+16 with the background versions) color codes.
/// </summary>
public enum Color16
{
	FGBlack = 30, BGBlack = 40,
	FGRed = 31, BGRed = 41,
	FGGreen = 32, BGGreen = 42,
	FGYellow = 33, BGYellow = 43,
	FGBlue = 34, BGBlue = 44,
	FGMagenta = 35, BGMagenta = 45,
	FGCyan = 36, BGCyan = 46,
	FGWhite = 37, BGWhite = 47,
	FGDefault = 39, BGDefault = 49,

	FGBBlack = 90, BGBBlack = 100,
	FGBRed = 91, BGBRed = 101,
	FGBGreen = 92, BGBGreen = 102,
	FGBYellow = 93, BGBYellow = 103,
	FGBBlue = 94, BGBBlue = 104,
	FGBMagenta = 95, BGBMagenta = 105,
	FGBCyan = 96, BGBCyan = 106,
	FGBWhite = 97, BGBWhite = 107,

	Reset = 0
}

/// <summary>
/// The text rendering mode.
/// </summary>
public enum ColorMode
{
	Normal,
	Bold,
	Dim,
	Italic,
	Underline,
	Blinking,
	Inverse = 7,
	Hidden,
	Strike
}