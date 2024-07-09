using System;


namespace Specter.Terminal.UI.Exceptions;


public class ComponentPropertyNotFoundException(string propertyName)
	: Exception($"Could not find property \"{propertyName}\".")
{
	string PropertyName { get; } = propertyName;
}

public class ComponentPropertyWrongTypeException(string propertyName, string typeName)
	: Exception($"\"{propertyName}\" property is not of type \"{typeName}\"")
{
	string PropertyName { get; } = propertyName;
	string TypeName { get; } = typeName;
}

// TODO: create a base class for these



public class ComponentPropertyAlreadyExists(string propertyName)
	: Exception($"The property \"{propertyName}\" already exists.")
{
	string PropertyName { get; } = propertyName;
}