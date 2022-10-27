using System.Reflection;

namespace SshService
{
    public static class Common
    {
        public static IConfigurationRoot? AppSetting { get; set; } = null;
        public static string? BasePath => Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        public static string AuthorizedKeysFilePath => AppSetting["AuthorizedKeysFile"];
    }
}