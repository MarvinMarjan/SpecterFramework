using Specter.Core;


namespace Specter.Terminal.UI.Exceptions;


public class AppException(string message)
	: SpecterException(message)
{}