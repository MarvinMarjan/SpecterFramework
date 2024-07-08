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


	private static object s_renderQueueLocker = new();

	private static List<Component> s_renderQueue = [];
	public static List<Component> RenderQueue // ! Use the property instead of the field.
	{
		get
		{
			lock(s_renderQueueLocker)
			{
				return s_renderQueue;
			}
		}

		private set
		{
			lock(s_renderQueueLocker)
			{
				s_renderQueue = value;
			}
		}
	}


	private static bool s_drawAllRequested = false;


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
			Terminal.Update();

			RunFrame(Terminal.TerminalResized);
		}
	}


	private void RunFrame(bool drawAll = false, bool? clearScreen = null)
	{
		clearScreen ??= drawAll;

		if (clearScreen is true)
			ClearAllScreen();

		Root.Update();

		if (drawAll || s_drawAllRequested)
			DrawAll(true);
		else
			DrawRenderQueue();

		s_drawAllRequested = false;
	}



	public static void AddComponentToRenderQueue(Component component)
	{
		if (!RenderQueue.Contains(component))
			RenderQueue.Add(component);
	}



	private static void DrawRenderQueue()
	{
		foreach (Component component in RenderQueue)
			Console.Write(component.Draw());

		RenderQueue.Clear();
	}


	public static void RequestDrawAll() => s_drawAllRequested = true;

	private void DrawAll(bool clearRenderQueue = false)
	{
		Console.Write(Root.Draw());

		if (clearRenderQueue)
			RenderQueue.Clear();
	}

	private static void ClearAllScreen()
	{
		StringBuilder codes = new();

		codes.Append(ControlCodes.CursorToHome());
		codes.Append(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.CursorUntilEnd));
		codes.Append(ControlCodes.EraseScreen(ControlCodes.ScreenErasingMode.SavedLines));

		Console.WriteLine(codes);
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
}
