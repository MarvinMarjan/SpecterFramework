using Specter.ANSI;


namespace Specter.Color.Chroma;


/// <summary>
/// Represents the start and end of a highlight operation.
/// </summary>
/// <param name="from"> The start. </param>
/// <param name="to"> The end. </param>
public readonly struct HighlightTarget(Token from, Token to)
{
	public Token From { get; init; } = from;
	public Token To { get; init; } = to;


	public HighlightTarget(Token token) : this(token, token)
	{}
	
}


public static class ChromaLang
{
	public static string? LastSource { get; private set; }


	/// <param name="source"> The string to be formatted. </param>
	/// <returns> A string formatted by Chroma. </returns>
	public static string Format(string source)
	{
		LastSource = source;

		var tokens = new Scanner().Scan(source);
		var structures = new StructureBuilder().BuildExpressionConvertableStructures(tokens);
		var expressions = ExpressionConverter.ConvertAll(structures);

		return Formatter.Format(expressions);
	}


	/// <summary>
	/// Same as 'ChromaLang.Format', but ignores any exception.
	/// </summary>
	/// <param name="source"> The string to be formatted. </param>
	/// <param name="output"> The out variable to store the result. </param>
	/// <returns> True if no exception was thrown, false otherwise. </returns>
	public static bool TryFormat(string source, out string? output)
	{
		try
		{
			output = Format(source);
			return true;
		}
		catch
		{
			output = null;
			return false;
		}
	}



	/// <summary>
	/// Highlights a HighlightTarget of the last string source
	/// Chroma formatted using a specific color.
	/// </summary>
	/// <param name="target"> The target to highlight. </param>
	/// <param name="color"> The color used to highlight. </param>
	/// <returns> The last source with the target highlighted. </returns>
	public static string HighlightTargetFromLastSource(HighlightTarget target, ColorObject? color = null)
	{
		if (LastSource is null)
			return string.Empty;

		string source = LastSource;

		color ??= ColorValue.FGRed;

		source = source.Insert(target.To.End, EscapeCodes.Reset);
		source = source.Insert(target.From.Start, color.AsSequence());

		return source;
	}
}