using System;
using System.Text;

using Specter.Color;
using Specter.String;
using Specter.Terminal.UI.Exceptions;


namespace Specter.Terminal.UI.System;


// TODO: try to improve this


public static class ExceptionMessageFormatter
{
	private static string ErrorSectionFrom(Exception exception, bool colon = false)
	{
		string exceptionType = (ColorValue.FGRed + ColorValue.Underline).Paint(GetExceptionTypeAsString(exception));
		string errorText = ColorValue.FGBRed.Paint(" Error");
		string separator = ColorValue.FGDefault.Paint(colon ? ":" : "");

		return exceptionType + errorText + separator;
	}



	public static string GetExceptionTypeAsString(Exception exception) => exception switch 
	{
		AppException => "App",
		ComponentException => "Component",
		ComponentPropertyException => "ComponentProperty",
		_ => "App"
	};



	public static string Format(AppException exception)
	{
		StringBuilder builder = new(ErrorSectionFrom(exception, true));

		builder.Append(" " + exception.Message);

		return builder.ToString();
	}


	public static string Format(ComponentException exception)
	{
		StringBuilder builder = new(ErrorSectionFrom(exception));

		string componentNameText = $"\"{exception.ComponentName}\"".FGGreen();

		builder.Append($" (Component {componentNameText}): ");
		builder.Append(exception.Message);

		return builder.ToString();
	}


	public static string Format(ComponentPropertyException exception)
	{
		StringBuilder builder = new(ErrorSectionFrom(exception));

		string propertyNameText = $"\"{exception.PropertyName}\"".FGGreen();
		string propertyTypeText = $"\"{exception.PropertyType}\"".FGYellow();
		string propertyTypeFullText = $"of type {propertyTypeText}";

		builder.Append(
			$" (Property {propertyNameText}{(exception.PropertyType is not null ? propertyTypeFullText : "")}): "
		);
		builder.Append(exception.Message);

		

		return builder.ToString();
	}
}