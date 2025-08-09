using System.Reflection;
using Fuzr.Core.Constants;

namespace Fuzr.Core.Helpers;

public static class ResourceHelper
{
    public static byte[] GetResourceBytes(string resourceName)
    {
        var s = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        var fullResourceName = $"{FuzrConstants.CoreResoucreNamespace}.{resourceName}";
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullResourceName);
        using var ms = new MemoryStream();
        stream?.CopyTo(ms);
        return ms.ToArray();
    }
}