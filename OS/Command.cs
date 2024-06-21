using System.Diagnostics;

namespace Specter.OS;


public class Command
{
	public static void Run(string command)
	{
		ProcessStartInfo info = new("/bin/bash")
		{
			Arguments = $"-c \"{command}\"",
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};

		Process process = new()
		{
			StartInfo = info
		};

		process.Start();
		process.WaitForExit();
	}
}