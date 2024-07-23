using System;
using System.Text;


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

		s_pinned.Text = value.ToString() ?? "";
		DrawPinned();

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



	internal static void DrawPinned()
		=> Console.Write(s_pinned?.Draw());
}