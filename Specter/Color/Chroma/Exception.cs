using System;


namespace Specter.Color.Chroma;


public class ChromaException : Exception
{
	public HighlightTarget? Target { get; set; }


	public ChromaException() : base() {}

	public ChromaException(HighlightTarget? target, string message) : base(message)
	{
		Target = target;
	}

	public ChromaException(HighlightTarget? target, string message, Exception inner) : base(message, inner)
	{
		Target = target;
	}
}