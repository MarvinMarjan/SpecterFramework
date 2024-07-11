using System.Collections.Generic;

using Specter.Terminal.UI.Exceptions;


namespace Specter.Terminal.UI.Components;


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
/// <param name="parent"> The component that the manager will be a child of </param>
/// <param name="requirement"> The initial requirements. </param>
public class ComponentPropertiesManager(Component parent, ComponentPropertyManagerRequirement? requirement = null)
	: IUpdateable
{
	public Component Parent { get; set; } = parent;
	
	public ComponentPropertyManagerRequirement Requirement { get; set; } = requirement ?? new(true, true);

	private List<ComponentProperty> _properties = [];



	public void Add(ComponentProperty property)
	{
		// TODO: check if this works
		if (_properties.Contains(property))
			throw new ComponentPropertyAlreadyExists(property.Name);

		_properties.Add(property);

		SetRequirementToProperty(property);
	}



	public T GetPropertyAs<T>(string propertyName) where T : class
	{
		foreach (ComponentProperty property in _properties)
			if (property.Name == propertyName && property is T convertedProperty)
				return convertedProperty;
			
		throw new ComponentPropertyNotFoundException(propertyName);
	}

	public ComponentProperty GetProperty(string propertyName)
		=> GetPropertyAs<ComponentProperty>(propertyName);



	public T[] GetAllPropertiesAs<T>() where T : class
		=> _properties.ToArray().PropertiesAs<T>();

	public ComponentProperty[] GetAllProperties()
		=> GetAllPropertiesAs<ComponentProperty>();


	
	public void SetRequirementToProperty(ComponentProperty property)
	{
		if (property is IInheritable inheritable)
		{
			inheritable.Inherit = Requirement.Inherit;
			inheritable.CanBeInherited = Requirement.CanBeInherited;
		}
	}

	public void SetRequirementToProperties(ComponentProperty[] properties)
	{
		foreach (ComponentProperty property in properties)
			SetRequirementToProperty(property);
	}

	public void SetRequirementToAllProperties() => SetRequirementToProperties([.. _properties]);



	public virtual void Update() => SetRequirementToAllProperties();
}