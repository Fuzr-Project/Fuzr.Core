using System.Drawing;
using Pastel;

namespace Fuzr.Core.Constants;

public static class FuzrConstants
{
    public const string CoreAppName = "Fuzr";
    public const string CoreResoucreNamespace = "Fuzr.Core.Apps";
    public const string AppRootPath = "apps";

    public const string CoreConfigFileName = "config.yaml";
    
    public static readonly string PoweredBy = $"  Powered by {CoreAppName}  "
        .Pastel(Color.CornflowerBlue)
        .PastelBg(Color.Black);
}