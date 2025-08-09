using Fuzr.Core.Models;
using Serilog;

namespace Fuzr.Core.Helpers;

public static class ArgumentsHelper
{
    public static void ValidateArguments(ref string[] args)
    {
        if (args.Length == 0)
        {
            Log.Information("No core app arguments provided.");
            args = ["help"];
        }
        else
        {
            Log.Information("Starting core app with args: " + string.Join(" ", args));
        }
    }
    
    public static List<Argument> ProcessArguments(string[] args)
    {
        var arguments = new List<Argument>();
        
        var stopProcessingSubCommand = false;
        foreach (var arg in args)
        {
            if (arg == "--")
            {
                stopProcessingSubCommand = true;
                continue;
            }
            
            var argObj = new Argument(arg, arguments.Count);

            if (argObj.IsBaseCommand || argObj.HasDashed || stopProcessingSubCommand)
            {
                arguments.Add(argObj);
                continue;
            }

            if (!(argObj.HasDashed))
            {
                var lastArg = arguments[^1];
                if (!lastArg.IsBaseCommand || !lastArg.HasValue || !stopProcessingSubCommand)
                    arguments[^1].Value = arg;
                else
                    arguments.Add(argObj);
            }
        }
        
        return arguments;
    }
}