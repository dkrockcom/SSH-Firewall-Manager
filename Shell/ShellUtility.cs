using System;
using System.Diagnostics;

namespace SshService.Shell
{
	public static class ShellUtility
	{
		public static ShellResult Run(string command, string arguments)
		{
			var result = new ShellResult();
			try
			{
				var process = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = command,
						Arguments = arguments,
						UseShellExecute = false,
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						CreateNoWindow = true
					}
				};

				process.Start();

				result.Result = process.StandardOutput.ReadToEnd();
				result.Error = process.StandardError.ReadToEnd();

				process.WaitForExit();

				result.ExitCode = process.ExitCode;
				return result;
			}
			catch (Exception e)
			{
				result.ExitCode = int.MaxValue;
				result.Exception = e;
			}
			return result;
		}
	}
}
