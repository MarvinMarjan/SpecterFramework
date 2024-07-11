using System;


namespace Specter.Terminal.UI.Exceptions;


public class ComponentException(string message)
	: Exception(message)
{}