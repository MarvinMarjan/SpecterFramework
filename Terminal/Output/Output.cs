using System;
using System.Text;

using Specter.Terminal.UI;


namespace Specter.Terminal.Output;


public static class TerminalStream
{
	private static PinnedText? s_pinned;



	public static void Write(object? value = null, bool newLine = false)
	{
		value ??= "";

		if (WriteToPinned(value))
			return;

		if (newLine)
			Console.WriteLine(value.ToString());
		else
			Console.Write(value.ToString());
	}

	public static void WriteLine(object? value = null)
		=> Write(value, true);



	private static bool WriteToPinned(object value)
	{
		if (s_pinned is null)
			return false;

		s_pinned.Write(value.ToString() ?? "");
		return true;
	}



	public static void ClearAllScreen()
	{
		StringBuilder codes = new();

		codes.Append(ControlCodes.CursorToHome());
		codes.Append(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd));
		codes.Append(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.SavedLines));

		Console.Write(codes);
	}



	public static void Pin()
		=> s_pinned = PinnedText.FromCurrent();

	public static void Unpin()
		=> s_pinned = null;


	
	/// <summary>
	/// Create an amount of lines.
	/// </summary>
	/// <param name="count"> The number of lines. </param>
	/// <returns> The final cursor position. </returns>
	public static Point AllocateLines(int count)
	{
		Write(new string('\n', count));
		Point cursorPos = TerminalAttributes.CursorPosition;
		cursorPos.Row -= (uint)count;

		TerminalAttributes.CursorPosition = cursorPos;

		return cursorPos;
	}
}