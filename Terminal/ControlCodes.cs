using Specter.ANSI;


namespace Specter.Terminal;


/// <summary>
/// Defines methods that use ANSI control codes for controlling the terminal behavior.
/// </summary>
public static class ControlCodes
{
	public enum ScreenErasingMode
	{
		CursorUntilEnd,
		CursorUntilBeginning,
		Full,
		SavedLines
	}


	public enum LineErasingMode
	{
		CursorUntilEnd,
		CursorUntilBeginning,
		Full
	}


	public static string CursorToHome() => EscapeCodes.EscapeCodeWithController + 'H';
	public static string CursorTo(uint line, uint column) => $"{EscapeCodes.EscapeCodeWithController}{line};{column}H";
	public static string CursorUp(uint lines) => $"{EscapeCodes.EscapeCodeWithController}{lines}A";
	public static string CursorDown(uint lines) => $"{EscapeCodes.EscapeCodeWithController}{lines}B";
	public static string CursorRight(uint columns) => $"{EscapeCodes.EscapeCodeWithController}{columns}C";
	public static string CursorLeft(uint columns) => $"{EscapeCodes.EscapeCodeWithController}{columns}D";
	public static string CursorToBeginningOfPreviousLine(uint lines) => $"{EscapeCodes.EscapeCodeWithController}{lines}E";
	public static string CursorToBeginningOfNextLine(uint lines) => $"{EscapeCodes.EscapeCodeWithController}{lines}F";
	public static string CursorToColumn(uint lines) => $"{EscapeCodes.EscapeCodeWithController}{lines}G";
	public static string SaveCursorPos() => $"{EscapeCodes.EscapeCodeWithController}s";
	public static string LoadCursorPos() => $"{EscapeCodes.EscapeCodeWithController}u";


	public static string EraseScreen(ScreenErasingMode mode = ScreenErasingMode.Full)
		=> $"{EscapeCodes.EscapeCodeWithController}{(int)mode}J";
	public static string EraseEntireScreen() => $"{EscapeCodes.EscapeCodeWithController}J";
	public static string EraseLine(LineErasingMode mode = LineErasingMode.Full)
		=> $"{EscapeCodes.EscapeCodeWithController}{(int)mode}K";
}