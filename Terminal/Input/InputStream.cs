using System;
using System.Collections.Generic;

using Specter.Color.Paint;


namespace Specter.Terminal.Input;


/// <summary>
/// Joins input receiving, key processment and visual formatting into a single class.
/// </summary>
public abstract class InputStream
{
	public delegate void KeyProcessor(ConsoleKeyInfo info);

	/// <summary>
	/// Stores processors linked to their corresponding key.
	/// </summary>
	public Dictionary<ConsoleKey, KeyProcessor> KeyProcessors { get; set; }

	/// <summary>
	/// Receiving input?
	/// </summary>
	protected bool Reading { get; set; }

	/// <summary>
	/// The string data received from stdin.
	/// </summary>
	private string _data;
	public string Data
	{
		get => _data;
		set
		{
			_data = value;
			Cursor.Index.Max = _data.Length;
		}
	}

	/// <summary>
	/// The cursor to be used.
	/// </summary>
	public Cursor Cursor { get; set; }


	public InputStream()
	{
		Cursor = new();
		Data = _data = "";
		KeyProcessors = [];
	}


	/// <summary>
	/// Starts reading from stdin.
	/// </summary>
	/// <returns> The inputted data. </returns>
	public abstract string Read();


	/// <summary>
	/// Reads each character when inputted.
	/// </summary>
	protected abstract string ReadData();

	/// <summary>
	/// Processes the character if there's a processor for it.
	/// </summary>
	/// <param name="key"> The ConsoleKey representation of the character. </param>
	/// <returns> True if it was processed, false otherwise. </returns>
	protected abstract bool ProcessKey(ConsoleKeyInfo info);

	/// <summary>
	/// Controls how the input data is showed to the terminal.
	/// </summary>
	/// <param name="source"> The current full string data. </param>
	/// <returns> A formatted (or not) string. </returns>
	protected abstract string Format(string source);
}




/// <summary>
/// The default implementation of InputStream.
/// </summary>
public class DefaultInputStream : InputStream
{
	/// <summary>
	/// The painter to be used when formatting.
	/// </summary>
	public RulePainter Painter { get; set; }

	new public Cursor Cursor
	{
		get => base.Cursor;
		set
		{
			Painter.Cursor = base.Cursor = value;
		}
	}


	public DefaultInputStream() : base()
	{
		Painter = new([]) {
			Cursor = Cursor
		};

		KeyProcessors.Add(ConsoleKey.Enter, _ => Reading = false);
		KeyProcessors.Add(ConsoleKey.LeftArrow, CursorLeft);
		KeyProcessors.Add(ConsoleKey.RightArrow, CursorRight);
		KeyProcessors.Add(ConsoleKey.Backspace, Backspace);
	}



	protected void CursorLeft(ConsoleKeyInfo info)
	{
		if (info.Modifiers.HasFlag(ConsoleModifiers.Control))
			Cursor.CursorPreviousWord();
		else
			Cursor.CursorToLeft();
	}

	protected void CursorRight(ConsoleKeyInfo info)
	{
		if (info.Modifiers.HasFlag(ConsoleModifiers.Control))
			Cursor.CursorNextWord();
		else
			Cursor.CursorToRight();
	}


	protected void Backspace(ConsoleKeyInfo info)
	{
		if (Data.Length == 0 || Cursor.Index.IsAtStart())
			return;

		bool controlPressed = info.Modifiers.HasFlag(ConsoleModifiers.Control);

		bool startedWithWord = char.IsLetterOrDigit(Cursor.PeekLeftChar());

		do
		{
			if (!startedWithWord)
			{
				Data = Data.Remove(--Cursor.Index, 1);
				break;
			}

			Data = Data.Remove(--Cursor.Index, 1);

			if (!controlPressed)
				break;
		}
		while (!Cursor.Index.IsAtStart() && char.IsLetterOrDigit(Cursor.PeekLeftChar()));
	}



	public override string Read()
	{
		Cursor.Stream = this;

		// disable the default cursor visibility. We are going to use our own customized cursor
		bool startCursorVisibility = TerminalAttributes.CursorVisible;
		TerminalAttributes.CursorVisible = false;

		string data = ReadData();

		// reset the default cursor visibility to the original state.
		TerminalAttributes.CursorVisible = startCursorVisibility;

		Console.Write('\n');

		return data;
	}



	protected override string ReadData()
	{
		Data = "";
		Reading = true;

		Console.Write(ControlCodes.SaveCursorPos());

		while (Reading)
		{
			WriteFormat(true);

			ConsoleKeyInfo info = Console.ReadKey(true);
			bool processed = ProcessKey(info);

			// don't insert keys that have a processor to Data.
			if (!processed)
			{
				Data = Data.Insert(Cursor.Index, info.KeyChar.ToString());
				Cursor.Index++;
			}
		}

		// writes the formatted data once again, but without drawing the cursor.
		WriteFormat(false);

		return Data;
	}


	protected override bool ProcessKey(ConsoleKeyInfo info)
	{
		// tries to get the corresponding key processor.
		if (KeyProcessors.TryGetValue(info.Key, out KeyProcessor? processor))
		{
			processor(info);
			return true;
		}

		return false;
	}
	

	protected override string Format(string source)
		=> Painter.Paint(source);


	protected string FormatWithoutCursor(string source)
	{
		Cursor? lastCursor = Painter.Cursor;
		Painter.Cursor = null;

		string formatted = Format(source);

		Painter.Cursor = lastCursor;
		return formatted;
	}



	protected void WriteFormat(bool drawCursor = true)
		=> Console.Write(
				ControlCodes.LoadCursorPos()
				+ ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd)
				+ (drawCursor ? Format(Data) : FormatWithoutCursor(Data))
			);
}