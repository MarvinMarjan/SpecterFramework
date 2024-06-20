using System;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;


public class App
{
	public SectionComponent RootComponent { get; set; } = new(null);


	public App()
	{
		RootComponent.Size = new((uint)Console.LargestWindowWidth, (uint)Console.LargestWindowHeight);
	}


	public void Run()
	{
		Console.Write(ControlCodes.CursorToHome());
		Console.Write(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd));

		RootComponent.Update();
		Console.Write(RootComponent.Draw());
	}
}
