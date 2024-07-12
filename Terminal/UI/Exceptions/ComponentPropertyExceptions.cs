using System;
using System.ComponentModel;


namespace Specter.Terminal.UI.Exceptions;


public class ComponentPropertyException(
	string propertyName,
	string? propertyType,
	Component? owner,
	
	string message

) : Exception(message)
{
	public string PropertyName { get; } = propertyName;
	public string? PropertyType { get; } = propertyType;

	public Component? Owner { get; } = owner;
}

// TODO: add "Component Owner { get; }"