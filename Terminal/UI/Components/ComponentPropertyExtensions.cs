using System.Linq;


namespace Specter.Terminal.UI.Components;


public static class ComponentPropertyExtensions
{
	public static T[] PropertiesAs<T>(this object[] properties) where T : class
		=> (from property in properties
				let convertedProperty = property as T
				where convertedProperty is not null
				select convertedProperty).ToArray();
}


public static class InheritableExtensions
{
	public static void SetInheritablesInherit(this IInheritable[] properties, bool inherit)
	{
		foreach (var property in properties)
			if (property is not null)
				property.Inherit = inherit;
	}


	public static void SetInheritablesCanBeInherited(this IInheritable[] properties, bool can)
	{
		foreach (IInheritable? property in properties)
			if (property is not null)
				property.CanBeInherited = can;
	}
}