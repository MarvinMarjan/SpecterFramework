using System;

using Specter.OS;
using Specter.Terminal.UI;


namespace Specter.Terminal;


public static class TerminalAttributes
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


	public static Point CursorPosition
	{
		set => Console.Write(ControlCodes.CursorTo(value.Row, value.Col));
		get
		{
			var (Left, Top) = Console.GetCursorPosition();
		
			//* ANSI escape codes positioning are 1-index based, but
			//* Console.GetCursorPosition() returns 0-index based values, so it's
			//* necessary to increment the values by one to avoid weird behavior.

			return new((uint)Top + 1, (uint)Left + 1);
		}
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


	public static Size TerminalSize
	{
		get => GetTerminalSize();
		set => Console.SetWindowSize((int)value.Width, (int)value.Height);
	}


	public delegate void TerminalResizedEventHandler();

	public static event TerminalResizedEventHandler? TerminalResizedEvent;

	private static void RaiseTerminalResizedEvent() => TerminalResizedEvent?.Invoke();


	private static void SetEchoEnabled(bool enabled) =>
		Command.Run($"stty {(enabled ? "" : "-")}echo");


	private static Size GetTerminalSize()
		=> new((uint)Console.WindowWidth, (uint)Console.WindowHeight);



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