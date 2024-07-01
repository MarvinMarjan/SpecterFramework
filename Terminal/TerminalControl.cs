using System;
using System.Text;

using Specter.ANSI;
using Specter.OS;
using Specter.Terminal.UI;


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


public class Terminal
{
	private static Size? s_lastTerminalSize = null;
	private static Size? s_currentTerminalSize = null;

	private static bool s_terminalResized = false;
	public static bool TerminalResized => s_terminalResized;



	public static void SetEchoEnabled(bool enabled) =>
		Command.Run($"stty {(enabled ? "" : "-")}echo");

	public static void SetCursorVisible(bool visible) => Console.CursorVisible = visible;
	
	public static void SetOutputEncoding(Encoding encoding) => Console.OutputEncoding = encoding;
	public static void SetInputEncoding(Encoding encoding) => Console.InputEncoding = encoding;

	public static Encoding GetOutputEncoding() => Console.OutputEncoding;
	public static Encoding GetInputEncoding() => Console.InputEncoding;


	public static Size GetTerminalSize()
		=> new((uint)Console.WindowWidth, (uint)Console.WindowHeight);


	// TODO: try to add events and work with threads


	/// <summary>
	/// Updates the static state of Terminal. It's required if you want to use TerminalResized stuff.
	/// </summary>
	public static void Update()
	{
		s_terminalResized = false;
		s_lastTerminalSize ??= Size.None;

		s_currentTerminalSize = GetTerminalSize();
		
		s_terminalResized = s_lastTerminalSize != s_currentTerminalSize;

		if (s_terminalResized)
			s_lastTerminalSize = s_currentTerminalSize;
	}
}