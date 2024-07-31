using System;
using System.Text;

using Specter.Color;
using Specter.Color.Paint;
using Specter.String;


namespace Specter;


public static class ExceptionMessageFormatter
{
	private static string ErrorSectionFrom(Exception exception, bool colon = false)
	{
		string exceptionType = (ColorValue.FGRed + ColorValue.Underline).Paint(GetExceptionTypeAsString(exception));
		string errorText = " Error".FGBRed();
		string separator = colon ? ":" : "";

		return exceptionType + errorText + separator;
	}



	public static string GetExceptionTypeAsString(Exception exception)
	{
		string name = exception.GetType().Name.Replace("Exception", "");

		if (name.Length == 0)
			return "Specter";

		return name;
	}



	public static string BuildErrorStringStructure(Exception exception, string? details, string? extra = null)
	{
		StringBuilder builder = new(ErrorSectionFrom(exception, details is null));

		builder.Append(details is not null ? $" ({details}):" : string.Empty);
		builder.Append($"\n  --->> ".FGBRed() + $"{exception.Message}");
		builder.Append($"\n\n{extra}");

		return builder.ToString();
	}
}