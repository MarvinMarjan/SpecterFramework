using System;
using System.Collections.Generic;


namespace Specter.Terminal.UI.Components;


public class ComponentPropertyNotFoundException(string propertyName)
	: Exception($"Could not find property \"{propertyName}\".") {}

public class ComponentPropertyWrongTypeException(string propertyName, string typeName)
	: Exception($"\"{propertyName}\" property is not of type \"{typeName}\"") {}

public class ComponentPropertyAlreadyExistsException(string propertyName)
	: Exception($"The property \"{propertyName}\" already exists.") {}


// TODO: maybe set this as an interface?
public class ComponentPropertiesManager
{
	private Dictionary<string, object> _properties;


	public ComponentPropertiesManager()
	{
		_properties = [];
	}


	public void Add(string propertyName, object value)
	{
		try
		{
			_properties.Add(propertyName, value);
		}
		catch (ArgumentException)
		{
			throw new ComponentPropertyAlreadyExistsException(propertyName);
		}
	}

	public bool TryAdd(string propertyName, object value)
		=> _properties.TryAdd(propertyName, value);
	


	public T Get<T>(string propertyName)
	{
		if (!_properties.ContainsKey(propertyName))
			throw new ComponentPropertyNotFoundException(propertyName);

		object property = _properties[propertyName];

		if (property is T convertedProperty)
			return convertedProperty;
	
		throw new ComponentPropertyWrongTypeException(propertyName, typeof(T).Name);
	}
}