using System.Drawing;
using System.Reflection;
using System.Text;
using Fuzr.Core.Commands.BaseCommand;
using Fuzr.Core.Helpers;
using Pastel;

namespace Fuzr.Core.Commands;

public class Help : FuzrBaseCommand
{
    public override required string FullName { get; set; } = "help";
    public override required string ShortName { get; set; } = "h";

    public override void ExecuteFull(string[]? args)
    {
        if (args?.Length == 1) 
            ShowDefaultHelp();
        else
            ShowCommandHelp(args);
    }

    private void ShowCommandHelp(string[]? args)
    {
        if (args?.Length != 2)
        {
            ShowDefaultHelp();
            return;
        }
        
        var coreConfig = AppsHelper.GetCoreConfig();
        var command = args[1];
        
        var appConfig = coreConfig.AppConfigs.FirstOrDefault(x => x.Name.Equals(command, StringComparison.OrdinalIgnoreCase));

        if (appConfig == null)
        {
            ShowDefaultHelp();
            return;
        }

        var helpText = $"""
                        
                        {$"  {appConfig.Name} v{appConfig.Version}  ".Pastel(Color.WhiteSmoke).PastelBg(Color.DarkSlateBlue)}
                        
                        {appConfig.FullDescription}
                        
                        {"  Help  ".Pastel(Color.WhiteSmoke).PastelBg(Color.DarkSlateBlue)}
                        
                        {appConfig.HelpText}
                        
                        """;

        Console.WriteLine(helpText);
    }

    private void ShowDefaultHelp()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();
        var appName = assemblyName.Name.Pastel(Color.CornflowerBlue);
        var appsText = new StringBuilder();
        var coreConfig = AppsHelper.GetCoreConfig();
        
        foreach (var appConfig in coreConfig.AppConfigs)
        {
           var currentAppName = appConfig.Name;
           var shortDescription = appConfig.ShortDescription;
           appsText.Append($"{currentAppName.Pastel(Color.Orange)}\n {shortDescription.Pastel(Color.DarkGray)}\n\n");
        }
     
        var helpText = $"""
                       
                       {"  Basic Usage  ".Pastel(Color.WhiteSmoke).PastelBg(Color.DarkSlateBlue)}

                       Use the following syntax:
                        ./{appName} [Command] [Options]
                       
                       {"  Available Commands  ".Pastel(Color.WhiteSmoke).PastelBg(Color.DarkSlateBlue)}
                       
                       {"help, -h, --help | [command]".Pastel(Color.Orange)}
                        {"Show this help message and exit".Pastel(Color.DarkGray)}
                         
                       {"version, -v, --version".Pastel(Color.Orange)}
                        {"Show the version number and exit".Pastel(Color.DarkGray)}
                       
                       {appsText.ToString().TrimEnd()}
                        
                       {"  Examples  ".Pastel(Color.WhiteSmoke).PastelBg(Color.DarkSlateBlue)}
                       
                       To show this help:
                        ./{appName} help
                        
                       To run "example" command help:
                        ./{appName} help example
                       
                       To run "example" script with options:
                        ./{appName} example --verbose --color "hello world!!"
                       
                       """;
            
        Console.WriteLine(helpText);
    }
}