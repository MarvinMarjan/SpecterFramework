using System;


namespace Specter.Terminal.UI.Exceptions;


public class AppException(string message)
	: Exception(message)
{}