using System;
using System.Collections.Generic;
using System.Text;

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



	/// <summary>
	/// Queue containing the Components that are going to be drawed/rendered.
	/// </summary>
	public static List<Component> RenderQueue { get; private set; } = [];


	public static bool DrawAllRequested { get; private set; } = false;


	public App()
	{
		Terminal.SetEchoEnabled(false);
		Terminal.SetCursorVisible(false);

		OutputEncoding = new UTF8Encoding();

		// on CTRL+C pressed
		Console.CancelKeyPress += delegate { OnExit(); };

		Exit = false;
		Root = new();
	
		Load();
	}



	protected virtual void Load() {}

	protected virtual void Start()
	{
		// first drawing
		Terminal.ClearAllScreen();
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
			Terminal.ClearAllScreen();

		if (drawAll || DrawAllRequested)
			DrawAll(true);
		else
			DrawRenderQueue();

		DrawAllRequested = false;
	}

	protected virtual void End()
		=> OnExit();
	


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
		Terminal.SetEchoEnabled(true);
		Terminal.SetCursorVisible(true);
	}
}
