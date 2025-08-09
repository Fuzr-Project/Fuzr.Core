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

    public bool MatchesAnyCommand(string[] args)
    {
        var arg = args.FirstOrDefault();
        
        if (string.IsNullOrEmpty(arg))
            return false;
        
        var hasShortCommand = arg.Trim().Equals(ShortName, StringComparison.InvariantCultureIgnoreCase);
        var hasLongCommand = arg.Trim().Equals(FullName, StringComparison.InvariantCultureIgnoreCase);
        return hasShortCommand || hasLongCommand;
    }
    
    public bool MatchesFullCommand(string[] args)
    {
        var arg = args.FirstOrDefault();
        
        if (string.IsNullOrEmpty(arg))
            return false;
        
        var hasLongCommand = arg.Trim().Equals(FullName, StringComparison.InvariantCultureIgnoreCase);
        return hasLongCommand;
    }
    
    public bool MatchesShortCommand(string[] args)
    {
        var arg = args.FirstOrDefault();
        
        if (string.IsNullOrEmpty(arg))
            return false;
        
        var hasShortCommand = arg.Trim().Equals(ShortName, StringComparison.InvariantCultureIgnoreCase);
        return hasShortCommand;
    }
}