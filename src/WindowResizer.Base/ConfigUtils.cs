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
                ProfilesFactory.Load(configPath);
                return true;
            }
            catch (Exception)
            {
                onError?.Invoke($"Could not Load config file from <{configPath}>, file not exists or not valid.");
                return false;
            }
        }

        public static bool LoadOrCreate(string? configPath, string? profileName, Action<string>? onError)
        {
            configPath ??= Path.Combine(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(WindowResizer)),
                DefaultConfigFile);

            profileName ??= string.Empty;

            try
            {
                ProfilesFactory.Load(configPath);
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    var cnf = ProfileConfig.NewConfig(profileName);
                }
                else
                {
                    onError?.Invoke("Unexpected error while trying to load config file at \"" + configPath + "\": " + ex.Message);
                    return false;
                }
            }

            ProfilesFactory.ConfigPath = configPath;
            return true;
        }

    }
}
