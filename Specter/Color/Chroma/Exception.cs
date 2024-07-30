using Specter.Core.Exception;


namespace Specter.Color.Chroma;


public class ChromaException(string message, HighlightTarget? target = null)
	: SpecterException(message)
{
	public HighlightTarget? Target { get; set; } = target;


	public override string ToString()
	{
		string? details = null;
		string? extra = null;

		if (Target is HighlightTarget target)
		{
			details = "Index: " + $"{Target?.From.Start};{Target?.To.End}";
			extra = ChromaLang.HighlightTargetFromLastSource(target, ColorValue.FGRed + ColorValue.Underline);
		}

		return ExceptionMessageFormatter.BuildErrorStringStructure(this, details, extra);
	}
}