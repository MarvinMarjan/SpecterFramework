using System.Collections.Generic;

namespace Specter.Debug.Prism.Commands;


public interface ICommand
{
	void Execute(List<object?> args);
}


public readonly struct CommandData(Location location, List<object?> arguments)
{
	public Location Location { get; init; } = location;
	public List<object?> Arguments { get; init; } = arguments;
}