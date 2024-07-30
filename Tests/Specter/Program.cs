using System;
using System.Linq;
using System.Threading.Tasks;

using Specter.Color;
using Specter.Color.Paint;
using Specter.Color.Chroma;
using Specter.String;
using Specter.Terminal.Input;
using Specter.Terminal.Output;
using Specter.Terminal.UI;
using Specter.Terminal.UI.Components;
using Specter.Terminal.UI.Application;
using Specter.Terminal;


namespace Specter.Tests;


public class SpecterTestingApp : App
{
	protected override void Load()
	{
		SectionComponent section = new(
			"MainSection",
			this,
			alignment: Alignment.Center,
			size: new(50, 25),
			color: 118,
			inheritProperties: false
		);

		section.Color.DefaultValue = 120;
		

		TextComponent dynamicText = new(
			"DynamicText",
			section,
			alignment: Alignment.Center,
			text: "Counter"
		);
		
		int counter = 0;

		dynamicText.UpdateEvent += delegate {
			dynamicText.Text.DefaultValue = $"Counter {counter++}";
		};
	}
}


public class SpecterTesting
{
	private static void T1(string text)
	{
		// foreground coloring
		Console.WriteLine(text.FGRed());
		Console.WriteLine(text.FGGreen());
		Console.WriteLine(text.FGBlue());
		
		// background coloring
		Console.WriteLine(text.BGRed());
		Console.WriteLine(text.BGGreen());
		Console.WriteLine(text.BGBlue());

		// foreground and background coloring
		Console.WriteLine(text.Paint(ColorValue.FGRed + ColorValue.BGBBlue));

		// foreground, background and color mode coloring
		Console.WriteLine(text.Paint(ColorValue.FGRed + ColorValue.BGBBlue + ColorValue.Italic));
	
		// RGB and 256 colors

		ColorRGB rgb = new(200, 100, 120);
		Console.WriteLine(text.Paint(rgb));

	
		// color objects

		ColorObject obj = ColorObject.FromColor16(Color16.FGYellow, Color16.BGMagenta);
		obj.Mode = ColorMode.Strike;

		Console.WriteLine(text.Paint(obj));

		
		// color patterns

		ColorPattern pattern = new([ ColorValue.FGRed, ColorValue.FGGreen, ColorValue.FGBlue ]);
		ColorPattern gradient = new(ColorObject.ArrayFromColor256Sequence(235, 255).ToList())
		{
			ResetMode = ResetMode.Revert
		};
		
		Console.WriteLine(text.Paint(pattern));
		Console.WriteLine(text.Paint(gradient));
	}

	
	private static void T2()
	{
		SpecterTestingApp app = new();
		app.SetThisAsCurrentApp();

		app.Run();
	}


	private static void T3()
	{
		DefaultInputStream stream = new();

		stream.Cursor.Color = ColorValue.Underline;

		stream.Painter = new([
			new EqualityRule(ColorValue.FGRed, ["red"]), 
			new EqualityRule(ColorValue.FGGreen, ["green"]),
			new EqualityRule(ColorValue.FGBlue, ["blue"])
		])
		{
			Cursor = stream.Cursor
		};

		Console.Write("Give me some input: ");

		Console.WriteLine($"entered: {stream.Read()}");
	}


	private static void T4()
	{
		Console.WriteLine("Program started...");

		string[] operatorsString = (from ch in "+-*/,.;:=([{}])&|!$?@<>" select ch.ToString()).ToArray();

		TokenTarget[] operators = (from operatorString in operatorsString select (TokenTarget)operatorString).ToArray();
		TokenTarget[] keywords = [
			"abstract", "as", "base", "bool", "break", "byte",
			"case", "catch", "char", "checked", "class", "const",
			"continue", "decimal", "default", "delegate", "do", "double",
			"else", "enum", "event", "explicit", "extern",
			"finally", "fixed", "float", "for", "foreach", "goto",
			"if", "implicit", "in", "int", "interface", "internal",
			"is", "lock", "long", "namespace", "new",
			"object", "operator", "out", "override", "params", "private",
			"protected", "public", "readonly", "ref", "return", "sbyte",
			"sealed", "short", "sizeof", "stackalloc", "static", "string",
			"struct", "switch", "this", "throw", "try",
			"typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
			"using", "virtual", "void", "volatile", "while", "add",
			"remove", "set", "get"
		];
		TokenTarget[] conditionals = [
			"true", "false", "null"
		];
		
		

		/*
			Laborum sit quis cillum excepteur quis magna nostrud ea.
			Quis occaecat ullamco culpa.
			Veniam quis culpa incididunt.
			Culpa culpa ex sit velit eiusmod nisi dolor.
			Labore cupidatat adipisicing magna.
			Ipsum qui occaecat culpa minim ipsum quis ipsum nisi amet minim ea.
			Voluptate veniam magna in pariatur mollit commodo.
			Proident magna est incididunt non deserunt sit aliqua incididunt eu occaecat labore.
			Incididunt anim ad aliquip ea pariatur aliqua non laboris.
			Est officia officia reprehenderit nulla.
		*/

		ColorObject commentsColors = ColorObject.FromColor256(243, null, ColorMode.Italic);

		RulePainter painter = new([
			new BetweenRule(commentsColors, new("/", "/"), new("\n") {ShouldIgnoreWhitespace = false}),
			new BetweenRule(commentsColors, new("/", "*"), new("*", "/")),
			new EqualityRule(205, ["#"]) { ExtraPaintLength = 1 },
			new EqualityRule(87, operators),
			new EqualityRule(177, keywords),
			new EqualityRule(215, conditionals),
			new BetweenRule(113, "\"", "\""),
			new ConditionalRule(217, new TokenIsNumber()),
			new ConditionalRule(215,
				new PreviousTokenIs(new LogicCondition(
					LogicCondition.LogicOperation.Or,
					new TokenIsTarget("new"), new TokenIsTarget("class")
				))
			),
			new ConditionalRule(78,
				new NextTokenIs(new TokenIsTarget("("))
				), 
			new EqualityRule(ColorObject.FromColor256(193, null, ColorMode.Italic), [new("")],
				new NextTokenIs(new TokenIsTarget("."))
			),
			new EqualityRule(ColorObject.FromColor256(203, null, ColorMode.Italic), [new("")],
				new PreviousTokenIs(new TokenIsTarget("."))
			),
		]);

		/* string text = File.ReadAllText("/home/marvin/Documentos/program/csharp/SpecterTesting/Program.cs");

		text = text.Replace("\t", "    ");

		while (true)
		{
			Console.WriteLine(painter.Paint(text));
			Console.ReadKey();

			Console.Clear();
		} */


		DefaultInputStream stream = new() { Painter = painter };
		painter.Cursor = stream.Cursor;
		stream.Cursor.Color = ColorMode.Inverse;

		Console.Write("> ");
		stream.Read();
	}

	private static void T5()
	{
		Console.WriteLine("Started!!");

		/* Console.WriteLine(ChromaLang.Format(
		@"<magenta> public static void <cyan>Main<default _ bold>() <yellow _ underline>...</>"
		)); */
		
		Console.WriteLine(ChromaLang.Format(
		@"<(255,100,0) red>hello, <(100,255,0a) default italic>world</> <116>116</> <216 _ underline>216</>!!"
		));
	}


	private static void T6()
	{
		TerminalAttributes.CursorVisible = false;

		PinnedText[] texts = PinnedText.CreateCount(4);

		Parallel.ForEach(texts, (text, _) =>
		{
			char[] chars = ['▁', '▂', '▃', '▄', '▅', '▆', '▇', '█'];

			while (true)
				for (int i = 0;; i++)
				{
					if (i >= chars.Length)
					{
						i = 0;
						chars = chars.Reverse().ToArray();
					}

					text.Write(chars[i].ToString() + " Loading...");

					for (int o = 0; o < 9999999; o++);
				}
		});
	}


	private static void Main(string[] args)
	{
		Console.WriteLine(Environment.CurrentDirectory);

		T4();
	}
}