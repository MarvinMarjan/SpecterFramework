using System;


namespace Specter.Terminal.UI.Application.Exceptions;


public class AppException(string message)
	: Exception(message)
{}