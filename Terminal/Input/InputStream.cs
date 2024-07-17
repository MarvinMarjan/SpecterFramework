using System;
using System.Collections.Generic;
using Specter.Color.Paint;


namespace Specter.Terminal.Input;


public abstract class InputStream
{
	public delegate void KeyProcessor();

	public Dictionary<ConsoleKey, KeyProcessor> KeyProcessors { get; set; }

	protected bool Reading { get; set; }

	protected string Data { get; set; }
	public Cursor Cursor { get; set; }


	public InputStream()
	{
		Data = "";
		Cursor = new();
		KeyProcessors = [];
	}


	public abstract string Read();


	protected abstract string ReadData();
	protected abstract bool ProcessChar(ConsoleKey ch);
	protected abstract string Format(string source);
}




public class DefaultInputStream : InputStream
{
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

		KeyProcessors.Add(ConsoleKey.Enter, () => Reading = false);
		KeyProcessors.Add(ConsoleKey.LeftArrow, () => Cursor.Index--);
		KeyProcessors.Add(ConsoleKey.RightArrow, () => Cursor.Index++);
		KeyProcessors.Add(ConsoleKey.Backspace,
		() => {
				if (Data.Length == 0 || Cursor.Index - 1 < 0)
					return;

				Data = Data.Remove(Cursor.Index - 1, 1);
				Cursor.Index--;
			}
		);
	}



	public override string Read()
	{
		bool startCursorVisibility = Terminal.CursorVisible;
		Terminal.CursorVisible = false;

		string data = ReadData();

		Terminal.CursorVisible = startCursorVisibility;

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
			bool processed = ProcessChar(info.Key);

			if (!processed)
			{
				Data = Data.Insert(Cursor.Index, info.KeyChar.ToString());
				Cursor.IndexLimit = Data.Length;
				Cursor.Index++;
			}

			else
				Cursor.IndexLimit = Data.Length;
		}

		WriteFormat(false);

		return Data;
	}

	protected override bool ProcessChar(ConsoleKey ch)
	{
		if (KeyProcessors.TryGetValue(ch, out KeyProcessor? processor))
		{
			processor();
			return true;
		}

		return false;
	}

	protected override string Format(string source)
		=> Painter.Paint(source);


	private void WriteFormat(bool drawCursor = true)
		=> Console.Write(
				ControlCodes.LoadCursorPos()
				+ ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd)
				+ (drawCursor ? Format(Data) : FormatWithoutCursor(Data))
			);


	private string FormatWithoutCursor(string source)
	{
		Cursor? lastCursor = Painter.Cursor;
		Painter.Cursor = null;

		string formatted = Format(source);

		Painter.Cursor = lastCursor;
		return formatted;
	}

}