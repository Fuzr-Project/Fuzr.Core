using System.Diagnostics;
using System.Reflection;
using System.Text;
using Fuzr.Core.Constants;
using Fuzr.Core.Models.Config;
using Serilog;
using YamlDotNet.Serialization;

namespace Fuzr.Core.Helpers;

public static class AppsHelper
{
    private static readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithCaseInsensitivePropertyMatching()
        .Build();

    private static readonly ISerializer _serializer = new SerializerBuilder()
        .Build();
    
    public static List<string> GetAppsConfigPaths(string folderPath)
    {
        var result = new List<string>();

        try
        {
            var files = Directory
                .EnumerateFiles(folderPath, "*.yaml", SearchOption.AllDirectories);
            result.AddRange(files);
        }
        catch (Exception ex)
        {
            Log.Error("{GetScriptsName}: {ExMessage})", nameof(GetAppsConfigPaths), ex.Message);
        }

        return result;
    }

    public static AppConfig? BuildApp(string appsRootPath, string configPath, string outputPath)
    {
        Log.Information("[{method}] Starting to build app...", nameof(BuildApp));
        
        var configParentPath = Directory.GetParent(configPath);
        
        if (configParentPath == null)
            return null;
        
        var appRelativePath = Path.GetRelativePath(appsRootPath, configParentPath.FullName);
        
        if (string.IsNullOrEmpty(appRelativePath))
            return null;
        
        var yamlText = File.ReadAllText(configPath);
        
        var appConfig = DeserializeAppConfig(yamlText);
        appConfig.RelativePath = appRelativePath;

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        
        var buildOutput = outputPath + "/" + appConfig.RelativePath;

        if (!Directory.Exists(buildOutput))
        {
            Directory.CreateDirectory(buildOutput);
        }
        
        var appFileFullPath = appConfig.GetFullFilePath(appsRootPath);
        
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build /p:DebugType=None /p:CopyOutputSymbolsToPublishDirectory=false -c Release --output \"{buildOutput}\" \"{appFileFullPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = psi;
        process.OutputDataReceived += (_, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data != null) Console.Error.WriteLine(e.Data); };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        
        var outputFiles = Directory
            .EnumerateFiles(buildOutput, "*", SearchOption.AllDirectories)
            .Select(x => Path.GetRelativePath(outputPath, x));
        
        appConfig.Assemblies.AddRange(outputFiles);
        
        return appConfig;
    }

    public static string BuildFinalConfig(List<AppConfig> appConfigs)
    {
        var coreConfig = new CoreConfig()
        {
            AppConfigs = appConfigs
        };
        return _serializer.Serialize(coreConfig);
    }

    public static CoreConfig DeserializeCoreConfig(byte[] appConfigBytes)
    {
        var configYaml = Encoding.UTF8.GetString(appConfigBytes);
        return _deserializer.Deserialize<CoreConfig>(configYaml);
    }
    
    public static AppConfig DeserializeAppConfig(string appConfigYaml)
    {
        return _deserializer.Deserialize<AppConfig>(appConfigYaml);
    }
    
    public static void RunApp(string[] args, AppConfig appConfig)
    {
        var appResourceName = appConfig.GetFullResourceName(FuzrConstants.CoreResoucreNamespace);
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(appResourceName);

        if (stream == null)
        {
            Log.Warning(FuzrErrors.UnknownCommand);
            return;
        }
        
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        var assemblyBytes = ms.ToArray();

        var assembly = Assembly.Load(assemblyBytes);
        
        AppDomain.CurrentDomain.AssemblyResolve += (_, dependencyArgs) =>
        {
            var dependencyAssemblyName = new AssemblyName(dependencyArgs.Name);
            var culture = string.IsNullOrEmpty(dependencyAssemblyName.CultureName) ? string.Empty : $".{dependencyAssemblyName.CultureName}";
            var resourceName = $"{appConfig.GetResourceNamespace(FuzrConstants.CoreResoucreNamespace)}{culture}.{dependencyAssemblyName.Name}.dll";
            using var dependencyStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (dependencyStream == null)
            {
                Log.Warning("[{method}] Failed to load assembly '{assemblyName}'.", "AssemblyResolve", dependencyAssemblyName);
                return null;
            }
            
            using var dependencyMs = new MemoryStream();
            dependencyStream.CopyTo(dependencyMs);
            return Assembly.Load(dependencyMs.ToArray());
        };
        
        var entryPoint = assembly.EntryPoint;
        if (entryPoint != null)
        {
            var paramLength = entryPoint.GetParameters().Length;
            var appArgs = args.Length > 1 ? args.Skip(1).ToArray() : [];
            var parameters = paramLength == 0 ? null : new object[] { appArgs };
            entryPoint.Invoke(null, parameters);
        }
    }
}