using System.Text.Json;

namespace Specter.Debug.Prism.Client;


public struct ClientDataTransferStructure(string name, string command)
{
	public string Name { get; set; } = name;
	public string Command { get; set; } = command;


	public readonly string ToJson()
		=> JsonSerializer.Serialize(this);

	public static ClientDataTransferStructure FromJson(string json)
		=> JsonSerializer.Deserialize<ClientDataTransferStructure>(json);
}