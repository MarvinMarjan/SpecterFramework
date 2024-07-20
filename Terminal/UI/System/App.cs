using System;
using System.Collections.Generic;
using System.Text;

using Specter.Core;
using Specter.Terminal.UI.Components;
using Specter.Terminal.UI.Exceptions;


namespace Specter.Terminal.UI.System;


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
	/// The current running App instance.
	/// </summary>
	private static App? s_current_app;
	public static App CurrentApp
	{
		get => s_current_app ?? throw new AppException("There's no current app.");
		set => s_current_app = value;
	}



	/// <summary>
	/// Queue containing the Components that are going to be drawed/rendered.
	/// </summary>
	public static List<Component> RenderQueue { get; private set; } = [];


	public static bool DrawAllRequested { get; private set; } = false;


	public App()
	{
		TerminalManager.EchoEnabled = false;
		TerminalManager.CursorVisible = false;
		Console.OutputEncoding = new UTF8Encoding();

		// on CTRL+C pressed
		Console.CancelKeyPress += delegate { OnExit(); };

		Exit = false;
		Root = new();
	
		Load();
	}



	/// <summary>
	/// Called after the construction of the class.
	/// </summary>
	protected virtual void Load() {}

	/// <summary>
	/// Called before the App starts the updating cycle.
	/// </summary>
	protected virtual void Start()
	{
		// first drawing
		TerminalManager.ClearAllScreen();
		Update();
		Draw();
	}

	/// <summary>
	/// Called every frame. Should update all the program.
	/// </summary>
	protected virtual void Update()
	{
		TerminalManager.Update();

		Root?.Update();
	}

	/// <summary>
	/// Called every frame. Should draw the components that need to be drawn.
	/// </summary>
	protected virtual void Draw()
	{
		bool drawAll = TerminalManager.TerminalResized;

		if (drawAll is true)
			TerminalManager.ClearAllScreen();

		if (drawAll || DrawAllRequested)
			DrawAll(true);
		else
			DrawRenderQueue();

		DrawAllRequested = false;
	}

	/// <summary>
	/// Called at the end of the program. Should deinitialize stuff and back to normal state.
	/// </summary>
	protected virtual void End()
		=> OnExit();
	


	/// <summary>
	/// Starts running the App.
	/// </summary>
	public void Run()
	{
		SetThisAsCurrentApp();

		try
		{
			Start();

			while (!Exit)
			{
				Update();
				Draw();
			}

			End();
		}
		
		catch (Exception e)
		{
			Log.Error(e);
		}

		finally
		{
			Console.ReadKey(true);
			End();
		}
	}


	
	public void SetThisAsCurrentApp() => CurrentApp = this;



	public Component? TryGetComponentByName(string name)
	{
		Component? component = null;

		Component.ForeachChildIn(Root, child => {
			if (child.Name == name)
				component = child;
		});

		return component;
	}

	public Component GetComponentByName(string name)
		=> TryGetComponentByName(name)
			?? throw new ComponentException(name, "The Component couldn't be found.");



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


	public static void RequestDrawAll() => DrawAllRequested = true;

	private void DrawAll(bool clearRenderQueue = false)
	{
		Console.Write(Root.Draw());

		if (clearRenderQueue)
			RenderQueue.Clear();
	}




	/// <summary>
	/// On exiting stuff.
	/// </summary>
	private static void OnExit()
	{
		Console.Write(ControlCodes.CursorToHome(), ControlCodes.EraseScreen());
		TerminalManager.EchoEnabled = true;
		TerminalManager.CursorVisible = true;
	}
}
