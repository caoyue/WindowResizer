using System;
using System.IO;
using WindowResizer.Configuration;

namespace WindowResizer.Base
{
    public static class ConfigUtils
    {
        private const string DefaultConfigFile = "WindowResizer.config.json";

        public static bool Load(string? configPath, Action<string>? onError)
        {
            configPath ??= Path.Combine(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(WindowResizer)),
                DefaultConfigFile);

            try
            {
                ConfigFactory.Load(configPath);
                return true;
            }
            catch (Exception)
            {
                onError?.Invoke($"Load config file {configPath} error, file not exists or not valid.");
                return false;
            }
        }
    }
}
