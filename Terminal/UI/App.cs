using System;
using System.Text;
using System.Threading;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;


// TODO: document (add comments) everything.

public class App
{
	public bool Exit { get; set; }

	// The base Component of an UI App
	public RootComponent Root { get; set; }
	public uint MillisecondsDelay = 100;

	public Encoding AppEncoding = new UTF8Encoding();


	public App()
	{
		Terminal.SetEchoEnabled(false);
		Terminal.SetCursorVisible(false);
		
		Terminal.SetOutputEncoding(AppEncoding);

		// on CTRL+C pressed
		Console.CancelKeyPress += delegate { OnExit(); };


		Exit = false;

		Root = new();
	}


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
