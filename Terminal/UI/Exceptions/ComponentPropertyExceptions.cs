using System;


namespace Specter.Terminal.UI.Exceptions;


public class ComponentPropertyException(string propertyName, string? propertyType, string message)
	: Exception(message)
{
	public string PropertyName { get; } = propertyName;
	public string? PropertyType { get; } = propertyType;
}

// TODO: add "Component Owner { get; }"