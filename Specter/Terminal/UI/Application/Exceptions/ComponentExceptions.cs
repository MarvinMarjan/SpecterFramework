using System;


namespace Specter.Terminal.UI.Application.Exceptions;


public class ComponentException(string componentName, string message)
	: Exception(message)
{
	public string ComponentName { get; } = componentName;
}