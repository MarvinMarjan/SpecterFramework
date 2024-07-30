namespace Specter.Debug.Prism.Server;


public static class ServerState
{
	public static readonly int Port = 25000;

	public static PrismServer? Server { get; set; }
}