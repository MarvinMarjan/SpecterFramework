using System;
using System.Security.Cryptography;
using System.Text;

using Specter.Color;
using Specter.String;
using Specter.Terminal.UI.Exceptions;


namespace Specter.Terminal.UI.System;


public static class ExceptionMessageFormatter
{
	private static string ErrorSectionFrom(Exception exception, bool colon = false)
	{
		string exceptionType = (ColorValue.FGRed + ColorValue.Underline).Paint(GetExceptionTypeAsString(exception));
		string errorText = " Error".FGBRed();
		string separator = colon ? ":" : "";

		return exceptionType + errorText + separator;
	}



	public static string GetExceptionTypeAsString(Exception exception) => exception switch 
	{
		AppException => "App",
		ComponentException => "Component",
		ComponentPropertyException => "ComponentProperty",
		_ => "App"
	};



	public static string Format<TException>(TException exception)
		where TException : Exception => exception switch
	{
		AppException newException => Format(newException),
		ComponentException newException => Format(newException),
		ComponentPropertyException newException => Format(newException),
		Exception newException => Format(newException)
	};


	public static string Format(Exception exception)
	{
		StringBuilder builder = new(ErrorSectionFrom(exception));

		builder.Append(" " + exception.Message);

		return builder.ToString();
	}


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

		// TODO: use owner too

		string propertyNameText = $"\"{exception.PropertyName}\"".FGGreen();
		string propertyTypeText = $"\"{exception.PropertyType}\"".FGYellow();
		string propertyTypeFullText = exception.PropertyType is not null ? $"of type {propertyTypeText}" : "";

		builder.Append($" (Property {propertyNameText}{propertyTypeFullText}): ");
		builder.Append(exception.Message);

		return builder.ToString();
	}
}