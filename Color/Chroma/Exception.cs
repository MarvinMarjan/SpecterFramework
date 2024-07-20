using System;


namespace Specter.Color.Chroma;


public class ChromaException(string message, Token? token = null)
	: Exception(message)
{
	public Token? Token { get; set; } = token;
}