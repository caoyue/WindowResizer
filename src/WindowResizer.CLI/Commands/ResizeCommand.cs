using System;
using System.CommandLine;
using System.IO;

namespace WindowResizer.CLI.Commands
{
    internal class ResizeCommand : Command
    {
        public ResizeCommand() : base("resize", "resize windows.")
        {
            var configOption = new ConfigOption();
            AddOption(configOption);
            var profileOption = new ProfileOption();
            AddOption(profileOption);
            var processOption = new ProcessOption();
            AddOption(processOption);

            this.SetHandler(ResizeHandle, configOption, profileOption, processOption);
        }

        private static void ResizeHandle(FileInfo config, string profile, string process)
        {
            if (config is null)
            {
                Output.Error("Config file does not exist.");
            }

            var fp = Core.WindowControl.Resizer.GetForegroundHandle();
            Console.WriteLine(fp);
        }
    }
}
