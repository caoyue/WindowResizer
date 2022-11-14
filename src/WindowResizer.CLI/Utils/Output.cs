using Spectre.Console;

namespace WindowResizer.CLI.Utils
{
    public static class Output
    {
        public static void Echo(string str)
        {
            AnsiConsole.MarkupLineInterpolated($"{str}");
        }

        public static void Error(string str)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error: {str}[/]");
        }
    }
}
