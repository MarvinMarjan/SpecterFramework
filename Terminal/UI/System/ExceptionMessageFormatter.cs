using System;
using System.Text;

using Specter.Color;
using Specter.Color.Paint;
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



	public static string BuildErrorStringStructure(Exception exception, string? details)
	{
		StringBuilder builder = new(ErrorSectionFrom(exception));

		builder.Append($" ({details}):");
		builder.Append($"\n\n  --->> ".FGBRed() + $"{exception.Message}");

		return builder.ToString();
	}



	public static string Format<TException>(TException exception)
		where TException : Exception => exception switch
	{
		AppException newException => Format(newException),
		ComponentException newException => Format(newException),
		ComponentPropertyException newException => Format(newException),
		Exception newException => Format(newException)
	};


	public static string Format(Exception exception)
		=> BuildErrorStringStructure(exception, null);


	public static string Format(AppException exception)
		=> BuildErrorStringStructure(exception, null);


	public static string Format(ComponentException exception)
		=> BuildErrorStringStructure(
			exception,
			"Component " + exception.ComponentName.Quote()
		);


	public static string Format(ComponentPropertyException exception)
		=> BuildErrorStringStructure(
			exception,
			"Property " + exception.PropertyName.Quote()
				+ (exception.PropertyType is not null ? " of type " + exception.PropertyType.Quote(ColorValue.FGYellow) : "")
				+ (exception.Owner is not null ? ", owned by Component " + exception.Owner.Name.Quote() : "")
		);
}