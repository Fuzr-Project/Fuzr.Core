namespace Fuzr.Core.Models.Config;

public record CoreConfig
{
    public List<AppConfig> AppConfigs { get; set; } = [];
}