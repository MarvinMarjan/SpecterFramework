using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI.Application.Exceptions;


public class ComponentPropertyException(
	string propertyName,
	string? propertyType,
	Component? owner,
	
	string message

) : SpecterException(message)
{
	public string PropertyName { get; } = propertyName;
	public string? PropertyType { get; } = propertyType;

	public Component? Owner { get; } = owner;
}