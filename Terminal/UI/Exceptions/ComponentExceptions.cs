using System;


namespace Specter.Terminal.UI.Exceptions;


public class ComponentException(string name, string message)
	: Exception(message)
{
	public string Name { get; } = name;
}