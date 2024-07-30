using System;

using Specter;
using Specter.Terminal;
using Specter.Terminal.Output;


namespace Specter.Debug;


/// <summary>
/// Specter logging class.
/// </summary>
public static class Log
{
	public static void FullscreenError<TException>(TException exception)
		where TException : SpecterException
	{
		TerminalStream.ClearAllScreen();
		Console.Write(ControlCodes.CursorToHome());
		Console.WriteLine(exception.ToString());
	}
}
