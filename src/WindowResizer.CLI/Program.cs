using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;
using WindowResizer.Base;
using WindowResizer.CLI.Commands;
using WindowResizer.CLI.Utils;

namespace WindowResizer.CLI
{
    internal static class Program
    {
        static Task<int> Main(string[] args)
        {
            DpiUtils.SetDpiAware();

            var rootCommand = new RootCommand($"{nameof(WindowResizer)} CLI.");
            rootCommand.AddCommand(new ResizeCommand());

            var parser = new CommandLineBuilder(rootCommand)
                         .UseDefaults()
                         .UseExceptionHandler((e, _) =>
                         {
                             Output.Error(e.Message);
                         }, 1)
                         .UseHelp(ctx =>
                         {
                             ctx.HelpBuilder.CustomizeLayout(
                                 c =>
                                     HelpBuilder.Default
                                                .GetLayout()
                                                .Skip(1)
                                                .Prepend(p =>
                                                    AnsiConsole.Write(new FigletText(nameof(WindowResizer)).LeftJustified().Color(Color.Blue)))
                             );
                         })
                         .Build();

            return parser.InvokeAsync(args);
        }
    }
}
