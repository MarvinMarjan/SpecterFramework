using System;

using Specter.Terminal;


namespace Specter.Core;


/// <summary>
/// Specter logging class.
/// </summary>
public static class Log
{
	public static void Error<TException>(TException exception)
		where TException : SpecterException
	{
		TerminalManager.ClearAllScreen();
		Console.Write(ControlCodes.CursorToHome());
		Console.WriteLine(exception.ToString());
	}
}
