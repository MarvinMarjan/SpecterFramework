using System;
using System.Collections.Generic;
using System.Text;

using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI;


/// <summary>
/// Initializes a terminal UI app.
/// </summary>
public abstract class App
{
	public bool Exit { get; set; }

	/// <summary>
	/// The base Component of an UI App
	/// </summary>
	public RootComponent Root { get; private set; }

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
	public static List<Component> RenderQueue
	{
		get => s_renderQueue;
		private set => s_renderQueue = value;
	}


	private static bool s_drawAllRequested = false;
	public static bool DrawAllRequested => s_drawAllRequested;


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



	protected virtual void Start()
	{
		// first drawing
		ClearAllScreen();
		Update();
		Draw();
	}

	protected virtual void Update()
	{
		Terminal.Update();

		Root?.Update();
	}

	protected virtual void Draw()
	{
		bool drawAll = Terminal.TerminalResized;

		if (drawAll is true)
			ClearAllScreen();

		if (drawAll || s_drawAllRequested)
			DrawAll(true);
		else
			DrawRenderQueue();

		s_drawAllRequested = false;
	}

	protected virtual void End()
		=> OnExit();
	


	public void Run()
	{
		Start();

		while (!Exit)
		{
			Update();
			Draw();
		}
	}


	public static implicit operator Component(App app) => app.Root;



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
