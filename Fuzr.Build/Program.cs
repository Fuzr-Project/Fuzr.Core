using Fuzr.Core.Constants;
using Fuzr.Core.Helpers;
using Fuzr.Core.Models.Config;
using Serilog;

namespace Fuzr.Build;

public class Program
{
    private static readonly string OutputDir = Directory.GetCurrentDirectory() + "/../../../../Fuzr.Core/Apps/";
    
    static void Main(string[] args)
    {
        LogHelper.RegisterLogging();
        Log.Verbose("Starting Fuzr build...");

        var arguments = ArgumentsHelper.ProcessArguments(args);
        var outputPath = arguments.FirstOrDefault(x => x.Name.Equals("output", StringComparison.OrdinalIgnoreCase))?.Value ?? OutputDir;
        var appConfigPaths = new List<(string appsRootPath, string appConfigPath)>();
        
        foreach (var arg in arguments.Where(x => x.Name.Equals("apps-path", StringComparison.OrdinalIgnoreCase)))
        {
            if (arg.HasValue)
            {
                Log.Verbose("Processing apps-path: '{path}'", arg.Value);
                var configPaths = AppsHelper.GetAppsConfigPaths(arg.Value);
                appConfigPaths.AddRange(configPaths.Select(x => new ValueTuple<string, string>(arg.Value, x)));
            }
            else
            {
                Log.Warning("Invalid script path");
            }
        }

        var appConfigs = new List<AppConfig>();
        foreach (var appConfigPath in appConfigPaths)
        {
            Log.Verbose("Processing config: {config}", appConfigPath);
            var appConfig = AppsHelper.BuildApp(appConfigPath.appsRootPath, appConfigPath.appConfigPath, outputPath);
            
            if (appConfig != null)
                appConfigs.Add(appConfig);
        }
        
        var finalConfig = AppsHelper.BuildFinalConfig(appConfigs);
        File.WriteAllText($"{outputPath}/{FuzrConstants.CoreConfigFileName}", finalConfig);
    }
}