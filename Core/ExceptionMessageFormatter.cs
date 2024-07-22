using System;
using System.Text;

using Specter.Color;
using Specter.Color.Chroma;
using Specter.Color.Paint;
using Specter.String;
using Specter.Terminal.UI.Exceptions;

using ChromaToken = Specter.Color.Chroma.Token;
using RuleToken = Specter.Color.Paint.Token;

namespace Specter.Core;


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
		// TODO: probably can be automatized.

		AppException => "App",
		ComponentException => "Component",
		ComponentPropertyException => "ComponentProperty",

		ChromaException => "Chroma",

		_ => "Specter"
	};



	public static string BuildErrorStringStructure(Exception exception, string? details, string? extra = null)
	{
		StringBuilder builder = new(ErrorSectionFrom(exception, details is null));

		builder.Append(details is not null ? $" ({details}):" : string.Empty);
		builder.Append($"\n\n  --->> ".FGBRed() + $"{exception.Message}");
		builder.Append($"\n\n\n{extra}");

		return builder.ToString();
	}



	public static string Format<TException>(TException exception)
		where TException : Exception => exception switch
	{
		AppException newException => Format(newException),
		ComponentException newException => Format(newException),
		ComponentPropertyException newException => Format(newException),
		ChromaException newException => Format(newException),
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
	{
		string typeOfText = exception.PropertyType is not null ? " of type " + exception.PropertyType.Quote(ColorValue.FGYellow) : string.Empty;
		string ownedByText = exception.Owner is not null ? ", owned by Component " + exception.Owner.Name.Quote() : string.Empty;

		string details = "Property " + exception.PropertyName.Quote() + typeOfText + ownedByText;

		return BuildErrorStringStructure(exception, details);
	}


	public static string Format(ChromaException exception)
	{
		string? details = null;
		string? extra = null;

		if (exception.Token is ChromaToken token)
		{
			details = "Index: " + $"{exception.Token?.Start};{exception.Token?.End}";
			extra = ChromaLang.HighlightTokenFromLastSource(token);
		}

		return BuildErrorStringStructure(exception, details, extra);
	}
}