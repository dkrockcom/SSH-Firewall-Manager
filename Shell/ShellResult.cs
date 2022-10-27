using System;

namespace SshService.Shell
{
    public class ShellResult
    {
        public int ExitCode { get; set; }
        public string Result { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;

        public Exception? Exception { get; set; } = null;
    }
}
