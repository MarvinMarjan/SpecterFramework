using System;

using Specter.Terminal;
using Specter.Terminal.Output;


namespace Specter.Debug;


/// <summary>
/// Specter logging class.
/// </summary>
public static class Log
{
	public static void FullscreenError(Exception exception)
	{
		TerminalStream.ClearAllScreen();
		Console.Write(ControlCodes.CursorToHome());
		Console.WriteLine(exception.ToString());
	}
}
