using System;
using System.Text;

using Specter.OS;
using Specter.Terminal.UI;


namespace Specter.Terminal;


public static class TerminalManager
{
	private static Size? s_lastTerminalSize = null;
	private static Size? s_currentTerminalSize = null;

	private static bool s_terminalResized = false;
	public static bool TerminalResized => s_terminalResized;

	private static bool s_cursorVisible = true;
	public static bool CursorVisible
	{
		get => s_cursorVisible;
		set => s_cursorVisible = Console.CursorVisible = value;
	}

	private static bool s_echoEnabled = true;
	public static bool EchoEnabled
	{
		get => s_echoEnabled;
		set
		{
			s_echoEnabled = value;
			SetEchoEnabled(value);
		}
	}


	public delegate void TerminalResizedEventHandler();

	public static event TerminalResizedEventHandler? TerminalResizedEvent;

	private static void RaiseTerminalResizedEvent() => TerminalResizedEvent?.Invoke();


	private static void SetEchoEnabled(bool enabled) =>
		Command.Run($"stty {(enabled ? "" : "-")}echo");


	public static Size GetTerminalSize()
		=> new((uint)Console.WindowWidth, (uint)Console.WindowHeight);


	public static void ClearAllScreen()
	{
		StringBuilder codes = new();

		codes.Append(ControlCodes.CursorToHome());
		codes.Append(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd));
		codes.Append(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.SavedLines));

		Console.WriteLine(codes);
	}


	/// <summary>
	/// Updates the static state of the Terminal class. It's required if you want to use TerminalResized stuff.
	/// </summary>
	public static void Update()
	{
		s_terminalResized = false;
		s_lastTerminalSize ??= Size.None;

		s_currentTerminalSize = GetTerminalSize();
		
		s_terminalResized = s_lastTerminalSize != s_currentTerminalSize;

		if (s_terminalResized)
		{
			s_lastTerminalSize = s_currentTerminalSize;
			RaiseTerminalResizedEvent();
		}
	}
}