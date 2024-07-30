using Specter.Core.Exception;


namespace Specter.Terminal.UI.Application.Exceptions;


public class AppException(string message)
	: SpecterException(message)
{}