using System.CommandLine;
using System.IO;
using System.Linq;

namespace WindowResizer.CLI.Commands
{
    public class ConfigOption : Option<FileInfo>
    {
        public const string DefaultConfigFile = "WindowResize.config.json";

        public ConfigOption() : base(
            aliases: new[]
            {
                "--config",
                "-c"
            },
            description: "Config file path, use config file in current directory if omitted.",
            parseArgument: result =>
            {
                var filePath = result.Tokens.Count == 0 ? DefaultConfigFile : result.Tokens.Single().Value;
                if (File.Exists(filePath))
                {
                    return new FileInfo(filePath);
                }

                result.ErrorMessage = "Config file does not exist";
                return null;
            })
        {
            IsRequired = false;
            AllowMultipleArgumentsPerToken = false;
        }
    }

    public class ProcessOption : Option<string>
    {
        public ProcessOption() : base(
            aliases: new[]
            {
                "--process",
                "-p"
            },
            description: "Process name, use current foreground process if omitted.")
        {
            IsRequired = false;
            AllowMultipleArgumentsPerToken = false;
        }
    }

    public class ProfileOption : Option<string>
    {
        public ProfileOption() : base(
            aliases: new[]
            {
                "--profile",
                "-P"
            },
            description: "Profile name, use current profile if omitted.")
        {
            IsRequired = false;
            AllowMultipleArgumentsPerToken = false;
        }
    }
}
