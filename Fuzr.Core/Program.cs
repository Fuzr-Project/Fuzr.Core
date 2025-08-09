using Fuzr.Core.Constants;
using Fuzr.Core.Helpers;
using Serilog;

namespace Fuzr.Core;

class Program
{
    static void Main(string[] args)
    {
        LogHelper.RegisterLogging();
        ArgumentsHelper.ValidateArguments(ref args);

        var internalCommands = FuzrCommandHelpers.ExecuteInternalCommands(args);
        if (internalCommands)
            return;
        
        var coreConfig = AppsHelper.GetCoreConfig();
        var argList = ArgumentsHelper.ProcessArguments(args);
        var firstCommand = argList.FirstOrDefault(x => x.IsBaseCommand);
        
        if (firstCommand == null)
            return;
        
        var appConfig = coreConfig.AppConfigs
            .FirstOrDefault(x => x.Name.Equals(firstCommand.Name, StringComparison.OrdinalIgnoreCase));
        
        if (appConfig == null)
        {
            Log.Warning(FuzrErrors.UnknownCommand);
            return;
        }
        
        AppsHelper.RunApp(args, appConfig);
    }

    
}