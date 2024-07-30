using SystemException = System.Exception;


namespace Specter;


public class SpecterException(string message)
	: SystemException(message)
{}
