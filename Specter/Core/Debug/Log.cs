using System;

using Specter.Core.Exception;
using Specter.Terminal;
using Specter.Terminal.Output;


namespace Specter.Core.Debug;


/// <summary>
/// Specter logging class.
/// </summary>
public static class Log
{
	public static void Error<TException>(TException exception)
		where TException : SpecterException
	{
		TerminalStream.ClearAllScreen();
		Console.Write(ControlCodes.CursorToHome());
		Console.WriteLine(exception.ToString());
	}
}
