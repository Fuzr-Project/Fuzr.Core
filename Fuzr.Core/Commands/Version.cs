using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using Fuzr.Core.Commands.BaseCommand;
using Pastel;

namespace Fuzr.Core.Commands;

public class Version : FuzrBaseCommand
{
    public override required string FullName { get; set; } = "version";
    public override required string ShortName { get; set; } = "v";
    
    public override void ExecuteShort(string[]? args)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();
        var name = assemblyName.Name!;
        var version = assemblyName.Version!.ToString();

        Console.WriteLine($"""
                           {name.Pastel(Color.CornflowerBlue)} {version.Pastel(Color.LightGreen)}
                           """);

    }

    public override void ExecuteFull(string[]? args)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();

        var info = $"""
                   
                   {"  Assembly Info  ".Pastel(Color.White).PastelBg(Color.DarkSlateBlue)}
                   Name: {assemblyName.Name.Pastel(Color.CornflowerBlue)}
                   Core Version: {assemblyName.Version!.ToString().Pastel(Color.LightGreen)}
                   Scripts Version: {CustomProperties.ScriptsVersion.Pastel(Color.LightSeaGreen)}
                   
                   {"  Runtime Info  ".Pastel(Color.White).PastelBg(Color.DarkSlateBlue)}
                   .NET Version: {Environment.Version}
                   Framework: {RuntimeInformation.FrameworkDescription}
                   Target OS: {RuntimeInformation.OSDescription}
                   OS Architecture: {RuntimeInformation.OSArchitecture}
                   Process Architecture: {RuntimeInformation.ProcessArchitecture}
                   Is 64-bit OS: {Environment.Is64BitOperatingSystem}
                   Is 64-bit Process: {Environment.Is64BitProcess}
                   
                   {"  Thread Info  ".Pastel(Color.White).PastelBg(Color.DarkSlateBlue)}
                   Thread Pool Threads: {ThreadPool.ThreadCount}
                   Processor Count: {Environment.ProcessorCount}
                   
                   {"  Misc Info  ".Pastel(Color.White).PastelBg(Color.DarkSlateBlue)}
                   Machine Name: {Environment.MachineName}
                   User Name: {Environment.UserName}
                   OS Version: {Environment.OSVersion}
                   Current Directory: {Environment.CurrentDirectory}
                   System Directory: {Environment.SystemDirectory}
                   Command Line: {Environment.CommandLine}
                   
                   """;
        
        Console.WriteLine(info);
    }
}