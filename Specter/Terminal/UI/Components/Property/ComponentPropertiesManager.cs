using System.Collections.Generic;

using Specter.Terminal.UI.Application.Exceptions;


namespace Specter.Terminal.UI.Components.Property;


/// <summary>
/// Class that is utilized to set requirement values to ComponentProperties
/// when they are added to a ComponentPropertyManager.
/// </summary>
public class ComponentPropertyManagerRequirement(bool inherit, bool canBeInherited)
{
	public bool Inherit { get; set; } = inherit;
	public bool CanBeInherited { get; set; } = canBeInherited;
}


/// <summary>
/// Manages component properties.
/// </summary>
/// <param name="owner"> The component that the manager will be a child of </param>
/// <param name="requirement"> The initial requirements. </param>
public class ComponentPropertiesManager(Component owner, ComponentPropertyManagerRequirement? requirement = null)
	: IUpdateable
{
	public Component Owner { get; set; } = owner;
	
	public ComponentPropertyManagerRequirement Requirement { get; set; } = requirement ?? new(true, true);

	private readonly List<ComponentProperty> _properties = [];



	/// <summary>
	/// Adds a new ComponentProperty to the manager.
	/// </summary>
	/// <param name="property"> The property to be added. </param>
	/// <exception cref="ComponentPropertyException"> Exception thrown if the property has already been added to the manager. </exception>
	public void Add(ComponentProperty property)
	{
		if (_properties.Contains(property))
			throw new ComponentPropertyException(
				property.GetData(),
				"Can't add a new property because it has already been added."
			);

		_properties.Add(property);

		SetRequirementToProperty(property);
	}



	/// <summary>
	/// Gets a ComponentProperty in the manager as a specific type, if it exists.
	/// </summary>
	/// <param name="propertyName"> The name of the property. </param>
	/// <returns> The property converted to the specified type. </returns>
	/// <exception cref="ComponentPropertyException"> Exception thrown if it couldn't find the property. </exception>
	public T GetPropertyAs<T>(string propertyName) where T : class
	{
		foreach (ComponentProperty property in _properties)
			if (property.Name == propertyName && property is T convertedProperty)
				return convertedProperty;
			
		throw new ComponentPropertyException(
			new(propertyName, null, null),
			"Property couldn't be found."
		);
	}

	/// <summary>
	/// Same as GetPropertyAs<T>, but it doesn't convert the property.
	/// </summary>
	public ComponentProperty GetProperty(string propertyName)
		=> GetPropertyAs<ComponentProperty>(propertyName);



	/// <returns> An array containing all the properties converted to a specific type. </returns>
	public T[] GetAllPropertiesAs<T>() where T : class
		=> _properties.ToArray().PropertiesAs<T>();

	/// <summary>
	/// Same as GetAllPropertiesAs<T>, but it doesn't convert the properties.
	/// </summary>
	public ComponentProperty[] GetAllProperties()
		=> GetAllPropertiesAs<ComponentProperty>();



	/// <summary>
	/// Applies this manager requirements to a specific property.
	/// </summary>
	/// <param name="property"> The property to apply the requirements. </param>
	public void SetRequirementToProperty(ComponentProperty property)
	{
		if (property.Attributes.IgnoreManagerRequirement)
			return;

		if (property is IInheritable && property.Attributes is InheritableComponentPropertyAttributes attributes)
		{
			attributes.Inherit = Requirement.Inherit;
			attributes.CanBeInherited = Requirement.CanBeInherited;
        }
	}

	/// <summary>
	/// Applies this manager requirements to specific properties.
	/// </summary>
	/// <param name="properties"> The array of properties to apply the requirement. </param>
	public void SetRequirementToProperties(ComponentProperty[] properties)
	{
		foreach (ComponentProperty property in properties)
			SetRequirementToProperty(property);
	}

	/// <summary>
	/// Applies the requirements to the properties in this manager.
	/// </summary>
	public void SetRequirementToAllProperties() => SetRequirementToProperties([.. _properties]);



	public virtual void Update() => SetRequirementToAllProperties();
}