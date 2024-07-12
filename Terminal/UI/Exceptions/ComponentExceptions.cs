using System;


namespace Specter.Terminal.UI.Exceptions;


public class ComponentException(string componentName, string message)
	: Exception(message)
{
	public string ComponentName { get; } = componentName;
}