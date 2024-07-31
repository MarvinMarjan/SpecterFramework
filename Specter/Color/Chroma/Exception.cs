namespace Specter.Color.Chroma;


public class ChromaException(string message, HighlightTarget? target = null)
	: SpecterException(message)
{
	public HighlightTarget? Target { get; set; } = target;
}