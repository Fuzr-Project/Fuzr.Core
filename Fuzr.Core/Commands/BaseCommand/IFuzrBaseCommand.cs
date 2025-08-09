namespace Fuzr.Core.Commands.BaseCommand;

public interface IFuzrBaseCommand
{
    public string ShortName { get; set; }
    public string FullName { get; set; }
    
    public bool MatchesFullCommand(string[] args);
    public bool MatchesShortCommand(string[] args);
    
    public void ExecuteFull(string[]? args = null);
    public void ExecuteShort(string[]? args = null);
}