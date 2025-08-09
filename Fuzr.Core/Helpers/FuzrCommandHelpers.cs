using System.Reflection;
using Fuzr.Core.Commands.BaseCommand;
using Serilog;

namespace Fuzr.Core.Helpers;

public static class FuzrCommandHelpers
{
    public static bool ExecuteInternalCommands(string[] args)
    {
        var executedInternalCommands = false;
        var interfaceType = typeof(IFuzrBaseCommand);
        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .ToList();
        
        Log.Verbose($"Loaded {types.Count} core commands");    
        
        foreach (var type in types)
        {
            var instance = (IFuzrBaseCommand)Activator.CreateInstance(type)!;

            if (instance.MatchesFullCommand(args))
            {
                Log.Verbose($"Executing full command for {type.Name}.");
                instance.ExecuteFull(args);
                executedInternalCommands = true;
                break;
            }

            if (instance.MatchesShortCommand(args))
            {
                Log.Verbose($"Executing short command for {type.Name}.");
                instance.ExecuteShort(args);
                executedInternalCommands = true;
                break;
            }
        }
        
        return executedInternalCommands;
    }
}