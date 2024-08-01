using System;


namespace Specter.Terminal.UI.Application.Exceptions;


public class ComponentException : Exception
{
	public string? ComponentName { get; }


	public ComponentException() : base() {}

	public ComponentException(string componentName, string message) : base(message)
	{
		ComponentName = componentName;
	}

	public ComponentException(string componentName, string message, Exception inner) : base(message, inner)
	{
		ComponentName = componentName;
	}
}