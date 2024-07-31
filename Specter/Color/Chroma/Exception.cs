using System;


namespace Specter.Color.Chroma;


public class ChromaException(string message, HighlightTarget? target = null)
	: Exception(message)
{
	public HighlightTarget? Target { get; set; } = target;
}