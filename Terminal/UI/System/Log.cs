using System;


namespace Specter.Terminal.UI.System;


/// <summary>
/// Specter logging class.
/// </summary>
public static class Log
{
	public static void Error<TException>(TException exception)
		where TException : Exception
	{
		Terminal.ClearAllScreen();
		Console.Write(ControlCodes.CursorToHome());
		Console.WriteLine(ExceptionMessageFormatter.Format(exception));
	}
}
