using SystemException = System.Exception;


namespace Specter.Core;


public class SpecterException(string message)
	: SystemException(message)
{
	public override string ToString()
		=> ExceptionMessageFormatter.BuildErrorStringStructure(this, null);
}
