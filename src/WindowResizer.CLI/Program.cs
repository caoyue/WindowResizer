using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;
using WindowResizer.CLI.Commands;

namespace WindowResizer.CLI
{
    internal class Program
    {
        static Task Main(string[] args)
        {
            var rootCommand = new RootCommand($"{nameof(WindowResizer)} CLI.");
            rootCommand.AddCommand(new ResizeCommand());

            var parser = new CommandLineBuilder(rootCommand)
                         .UseDefaults()
                         .UseExceptionHandler((e, _) =>
                         {
                             Output.Error(e.Message);
                         }, 1)
                         .Build();

            if (!args.Any())
            {
                AnsiConsole.Write(new FigletText(nameof(WindowResizer)).LeftAligned().Color(Color.Blue));
                AnsiConsole.WriteLine();
                args = new[]
                {
                    "--help"
                };
            }

            return parser.InvokeAsync(args);
        }
    }
}
