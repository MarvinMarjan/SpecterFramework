using System;

using Specter.Terminal.UI.Components.Property;


namespace Specter.Terminal.UI.Application.Exceptions;


public class ComponentPropertyException : Exception
{
	public ComponentPropertyData PropertyData { get; set; }


	public ComponentPropertyException() : base() {}

	public ComponentPropertyException(ComponentPropertyData data, string message) : base(message)
	{
		PropertyData = data;
	}

	public ComponentPropertyException(ComponentPropertyData data, string message, Exception inner) : base(message, inner)
	{
		PropertyData = data;
	}
}