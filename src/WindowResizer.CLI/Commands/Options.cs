using System.CommandLine;
using System.IO;
using System.Linq;

namespace WindowResizer.CLI.Commands
{
    public class ConfigOption : Option<FileInfo>
    {
        public ConfigOption() : base(
            aliases: new[]
            {
                "--config",
                "-c"
            },
            description: "Config file path, use current config file if omitted.",
            parseArgument: result =>
            {
                if (result.Tokens.Count == 0)
                {
                    return null;
                }

                var filePath = result.Tokens.Single().Value;
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

    public class ProcessOption : Option<string>
    {
        public ProcessOption() : base(
            aliases: new[]
            {
                "--process",
                "-p"
            },
            description: "Process name, use foreground process if omitted.")
        {
            IsRequired = false;
            AllowMultipleArgumentsPerToken = false;
        }
    }

    public class TitleOption : Option<string>
    {
        public TitleOption() : base(
            aliases: new[]
            {
                "--title",
                "-t"
            },
            description: "Process title, all windows of the process will be resized if not specified.")
        {
            IsRequired = false;
            AllowMultipleArgumentsPerToken = false;
        }
    }
}
