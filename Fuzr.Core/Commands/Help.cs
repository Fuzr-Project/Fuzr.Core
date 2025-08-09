using Fuzr.Core.Commands.BaseCommand;
using Pastel;

namespace Fuzr.Core.Commands;

public class Help : FuzrBaseCommand
{
    public override required string FullName { get; set; } = "help";
    public override required string ShortName { get; set; } = "h";

    public override void ExecuteFull(string[]? args)
    {
        Console.WriteLine("Usage: help [command] [arguments]".Pastel(ConsoleColor.DarkGreen));
    }
}