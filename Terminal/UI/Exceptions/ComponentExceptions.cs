using Specter.Core;
using Specter.String;


namespace Specter.Terminal.UI.Exceptions;


public class ComponentException(string componentName, string message)
	: SpecterException(message)
{
	public string ComponentName { get; } = componentName;


	public override string ToString()
		=> ExceptionMessageFormatter.BuildErrorStringStructure(
			this,
			"Component " + ComponentName.Quote()
		);
	
}