using System;
using System.Linq;

using Specter.Core;
using Specter.ANSI;


namespace Specter.Color.Chroma;


public static class ChromaLang
{
	public static string? LastSource { get; set; }


	public static string Format(string source)
	{
		try
		{
			LastSource = source;
	
			var tokens = new Scanner().Scan(source);
			var structures = new StructureBuilder().BuildExpressionConvertableStructures(tokens);
			var expressions = ExpressionConverter.Convert(structures);
	
			return Formatter.Format(expressions);
		}
		
		catch (ChromaException e)
		{
			Log.Error(e);
		}

		return string.Empty;
	}


	public static bool TryFormat(string source, out string? output)
	{
		try
		{
			output = Format(source);
			return true;
		}
		catch (Exception)
		{
			output = null;
			return false;
		}
	}



	public static string HighlightTokenFromLastSource(Token token)
	{
		if (LastSource is null)
			return string.Empty;

		string source = LastSource;

		source = source.Insert(token.End, EscapeCodes.Reset);
		source = source.Insert(token.Start, ColorValue.FGRed.AsSequence());

		return source;
	}
}