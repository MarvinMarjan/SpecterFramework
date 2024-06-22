using System;
using System.Text;
using System.Threading;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;


public class App
{
	// The base Component of an UI App
	public SectionComponent RootComponent { get; set; }
	public uint MillisecondsDelay = 100;

	public Encoding AppEncoding = new UTF8Encoding();


	public App()
	{
		RootComponent = new(null);

		RootComponent.PropertiesCanBeInherited(false);
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
		// erase all terminal text
		Console.Write(ControlCodes.CursorToHome());
		Console.Write(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd));

		RootComponent.Update();
		Console.Write(RootComponent.Draw());

		Thread.Sleep((int)MillisecondsDelay);
	}
}
