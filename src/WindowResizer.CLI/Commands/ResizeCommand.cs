using System.CommandLine;
using WindowResizer.Base;
using WindowResizer.CLI.Utils;

namespace WindowResizer.CLI.Commands
{
    internal class ResizeCommand : Command
    {
        public ResizeCommand() : base("resize", "Resize window by process and window title.")
        {
            var configOption = new ConfigOption();
            AddOption(configOption);
            var profileOption = new ProfileOption();
            AddOption(profileOption);
            var processOption = new ProcessOption();
            AddOption(processOption);
            var titleOption = new TitleOption();
            AddOption(titleOption);

            this.SetHandler((config, profile, process, title) =>
            {
                WindowCmd.Resize(config?.FullName, profile, process, title, Output.Error);
            }, configOption, profileOption, processOption, titleOption);
        }
    }
}
