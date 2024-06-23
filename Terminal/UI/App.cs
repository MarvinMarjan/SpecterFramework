using System;
using System.Text;
using System.Threading;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;


public class App
{
	public bool Exit { get; set; }

	// The base Component of an UI App
	public SectionComponent RootComponent { get; set; }
	public uint MillisecondsDelay = 100;

	public Encoding AppEncoding = new UTF8Encoding();


	public App()
	{
		Exit = false;

		RootComponent = new(null);

		RootComponent.SetPropertiesCanBeInherited(false);
		RootComponent.Size.Value = new Size((uint)Console.LargestWindowWidth, (uint)Console.LargestWindowHeight);
		RootComponent.DrawBorder.Value = false;

		Terminal.SetEchoEnabled(false);
		Terminal.SetCursorVisible(false);
		
		Terminal.SetOutputEncoding(AppEncoding);

		// on CTRL+C pressed
		Console.CancelKeyPress += delegate { OnExit(); };
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

		RootComponent.Update();
		Console.Write(RootComponent.Draw());

		Thread.Sleep((int)MillisecondsDelay);
	}
}
