using System;

using Specter.Debug.Prism.Commands;


namespace Specter.Debug.Prism.Exceptions;


[Serializable]
public class CommandException : Exception
{
	public CommandException() { }
	public CommandException(string message) : base(message) { }
	public CommandException(string message, Exception inner) : base(message, inner) { }
}



[Serializable]
public class InvalidTokenException : CommandException
{
	public InvalidTokenException() { }
	public InvalidTokenException(string message) : base(message) { }
	public InvalidTokenException(string message, Exception inner) : base(message, inner) { }
}



[Serializable]
public class CommandNotFoundException : CommandException
{
	public string? Command { get; init; }


	public CommandNotFoundException() { }
	public CommandNotFoundException(string command, string message) : base(message)
	{
		Command = command;
	}

	public CommandNotFoundException(string command, string message, Exception inner) : base(message, inner)
	{
		Command = command;
	}
}