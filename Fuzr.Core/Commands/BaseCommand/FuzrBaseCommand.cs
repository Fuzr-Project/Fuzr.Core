namespace Fuzr.Core.Commands.BaseCommand;

/***
 * Fuzr.Core internal commands
 */
public abstract class FuzrBaseCommand : IFuzrBaseCommand
{
    public virtual required string ShortName { get; set; }
    public virtual required string FullName { get; set; }

    public abstract void ExecuteFull(string[]? args);
    public virtual void ExecuteShort(string[]? args) => ExecuteFull(args);

    public bool MatchesFullCommand(string[] args)
    {
        var arg = args.FirstOrDefault();
        
        if (string.IsNullOrEmpty(arg))
            return false;
        
        if (arg.StartsWith("-"))
        {
            if (!arg.StartsWith("--"))
                return false;
            arg = arg.Substring(2);
        }

        return arg.Equals(FullName, StringComparison.InvariantCultureIgnoreCase);
    }

    public bool MatchesShortCommand(string[] args)
    {
        var arg = args.FirstOrDefault();
        
        if (string.IsNullOrEmpty(arg))
            return false;
        
        if (!arg.StartsWith("-") || arg.StartsWith("--"))
            return false;

        var trimmedArg = arg.Substring(1);

        return trimmedArg.Equals(ShortName, StringComparison.InvariantCultureIgnoreCase);
    }
}