using Specter.Core;
using Specter.Color;
using Specter.String;
using Specter.Terminal.UI.Components;


namespace Specter.Terminal.UI.Exceptions;


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


	public override string ToString()
	{
		string typeOfText = PropertyType is not null ? " of type " + PropertyType.Quote(ColorValue.FGYellow) : string.Empty;
		string ownedByText = Owner is not null ? ", owned by Component " + Owner.Name.Quote() : string.Empty;

		string details = "Property " + PropertyName.Quote() + typeOfText + ownedByText;

		return ExceptionMessageFormatter.BuildErrorStringStructure(this, details);
	}
}