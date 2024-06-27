using System;
using System.Text;
using System.Threading;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;


// TODO: document (add comments) everything.

/// <summary>
/// Initializes a terminal UI app.
/// </summary>
public class App
{
	public bool Exit { get; set; }

	/// <summary>
	/// The base Component of an UI App
	/// </summary>
	public RootComponent Root { get; set; }

	/// <summary>
	/// The delay between each update and draw.
	/// </summary>
	public uint MillisecondsDelay { get; set; } = 100;

	public static Encoding AppEncoding
	{
		get => Console.OutputEncoding;
		set => Console.InputEncoding = value;
	}


	public App()
	{
		Terminal.SetEchoEnabled(false);
		Terminal.SetCursorVisible(false);

		AppEncoding = new UTF8Encoding();

		// on CTRL+C pressed
		Console.CancelKeyPress += delegate { OnExit(); };

		Exit = false;
		Root = new();
	}


	/// <summary>
	/// On exiting stuff.
	/// </summary>
	private static void OnExit()
	{
		Console.Write(ControlCodes.CursorToHome(), ControlCodes.EraseScreen());
		Terminal.SetEchoEnabled(true);
		Terminal.SetCursorVisible(true);
	}



	public void Run()
	{
		while (!Exit)
			RunFrame();
	}


	private void RunFrame()
	{
		// erase all terminal text
		Console.Write(ControlCodes.CursorToHome());
		Console.Write(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd));

		Root.Update();
		Console.Write(Root.Draw());

		Thread.Sleep((int)MillisecondsDelay);
	}
}
