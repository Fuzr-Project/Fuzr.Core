namespace Fuzr.Core.Models;

public record Argument
{
    public string RawName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int Order { get; set; }
    
    public bool HasDashed => RawName.StartsWith("-");
    public bool HasSingleDash => RawName.StartsWith("-");
    public bool HasDoubleDash => RawName.StartsWith("--");
    public bool HasSlash => RawName.StartsWith("/");
    public bool HasValue => !string.IsNullOrEmpty(Value);
    
    public bool IsBaseCommand => Order == 0;
    public bool IsSubCommand { get; set; }

    public Argument()
    {
    }
    
    public Argument(string argument, int order, string? value = null)
    {
        RawName = argument;
        Order = order;

        var trim = argument.Trim();
        
        if (trim.StartsWith("--"))
            trim = trim.Substring(2);
        
        if (trim.StartsWith("-") || trim.StartsWith("/"))
            trim = trim.Substring(1);
        
        Name = trim;
        
        if (Name.Contains("="))
        {
            var split = trim.Split("=");
            if (split.Length == 2)
            {
                Name = split[0].Trim();
                Value = split[1].Trim();
            }
        }
    }
}