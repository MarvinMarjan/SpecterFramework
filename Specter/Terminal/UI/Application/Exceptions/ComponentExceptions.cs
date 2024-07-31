namespace Specter.Terminal.UI.Application.Exceptions;


public class ComponentException(string componentName, string message)
	: SpecterException(message)
{
	public string ComponentName { get; } = componentName;
}