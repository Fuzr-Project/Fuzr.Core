namespace Fuzr.Core.Models.Config;

public record AppConfig
{
    // Public 
    public string Name { get; set; } = "hello-world";
    public string Version { get; set; } = "1.0.0";
    public List<string> Categories { get; set; } = new()
    {
        "test",
        "hello-world",
        "devops"
    };
    
    public string ShortDescription { get; set; } = "hello-world app short description";
    public string FullDescription { get; set; } = "hello-world app full description";
    public string HelpText { get; set; } = "hello-world app help text";
    
    public bool NativeAssembly { get; set; } = false;
    
    // Private
    public List<string> Assemblies { get; set; } = new();
    public string RelativePath { get; set; } = string.Empty;

    public string GetFullFilePath(string rootPath) => $"{rootPath}/{RelativePath}/{Name}.cs";
    public string GetFullResourceName(string assemblyNamespace) => $"{GetResourceNamespace(assemblyNamespace)}.{Name}.dll";
    public string GetResourceNamespace(string assemblyNamespace) => $"{assemblyNamespace}.{RelativePath}";
}