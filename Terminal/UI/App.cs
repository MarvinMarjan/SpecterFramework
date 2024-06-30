using System;
using System.Collections.Generic;
using System.Text;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;


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
	//public uint MillisecondsDelay { get; set; } = 100;


	public static Encoding OutputEncoding
	{
		get => Terminal.GetOutputEncoding();
		set => Terminal.SetOutputEncoding(value);
	}

	public static Encoding InputEncoding
	{
		get => Terminal.GetInputEncoding();
		set => Terminal.SetInputEncoding(value);
	}


	private static List<Component> s_renderQueue = [];
	public static Component[] RenderQueue => [.. s_renderQueue];

	private static bool s_renderQueueClosedForOneFrame = false;



	public App()
	{
		Terminal.SetEchoEnabled(false);
		Terminal.SetCursorVisible(false);

		OutputEncoding = new UTF8Encoding();

		// on CTRL+C pressed
		Console.CancelKeyPress += delegate { OnExit(); };

		Exit = false;
		Root = new();
	}




	public void Run()
	{
		// first drawing
		ClearAllScreen();
		RunFrame(true);

		while (!Exit)
		{
			// TODO: resizing is drawing components weird

			if (Terminal.TerminalResized())
				ClearAndDrawAll();

			RunFrame();
		}
	}


	private void RunFrame(bool drawAll = false)
	{
		Root.Update();

		if (drawAll)
			DrawAll(true);
		else
			DrawRenderQueue();

		if (s_renderQueueClosedForOneFrame)
			s_renderQueueClosedForOneFrame = false;
	}


	public static void AddComponentToRenderQueue(Component component)
	{
		if (!s_renderQueue.Contains(component) && !s_renderQueueClosedForOneFrame)
			s_renderQueue.Add(component);
	}

	private static void CloseRenderQueueForOneFrame() => s_renderQueueClosedForOneFrame = true;


	private static void DrawRenderQueue()
	{
		foreach (Component component in s_renderQueue)
			Console.Write(component.Draw());

		s_renderQueue.Clear();
	}

	private void ClearAndDrawAll(bool clearRenderQueue = true)
	{
		ClearAllScreen();
		DrawAll(clearRenderQueue);
	}

	private void DrawAll(bool clearRenderQueue = false)
	{
		Console.Write(Root.Draw());

		if (clearRenderQueue)
		{
			s_renderQueue.Clear();
			CloseRenderQueueForOneFrame();
		}

	}

	private static void ClearAllScreen()
	{
		Console.Write(ControlCodes.CursorToHome());
		Console.Write(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd));
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


	// TODO: only update parts that need to be updated
}
