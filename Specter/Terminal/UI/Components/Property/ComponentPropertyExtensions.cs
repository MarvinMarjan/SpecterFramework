using System.Linq;


namespace Specter.Terminal.UI.Components.Property;


public static class ComponentPropertyExtensions
{
	public static T[] PropertiesAs<T>(this object[] properties) where T : class
		=> (from property in properties
				let convertedProperty = property as T
				where convertedProperty is not null
				select convertedProperty).ToArray();



	public static InheritableComponentPropertyAttributes? GetPropertyAttributesFromInheritable(IInheritable inheritable)
	{
		var castedProperty = inheritable as ComponentProperty;
		var castedAttributes = castedProperty?.Attributes as InheritableComponentPropertyAttributes;

		return castedAttributes;
	}

	
	public static void SetInheritablePropertiesInherit(this IInheritable[] properties, bool inherit)
	{
		foreach (var property in properties)
		{
			var attributes = GetPropertyAttributesFromInheritable(property);

			if (attributes is not null)
				attributes.Inherit = inherit;
		}
	}


	public static void SetInheritablePropertiesCanBeInherited(this IInheritable[] properties, bool canBeInherited)
	{
		foreach (IInheritable? property in properties)
		{
			var attributes = GetPropertyAttributesFromInheritable(property);

			if (attributes is not null)
				attributes.CanBeInherited = canBeInherited;
		}
	}
}