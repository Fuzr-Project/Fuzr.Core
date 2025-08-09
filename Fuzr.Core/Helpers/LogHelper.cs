using Serilog;
using Serilog.Events;

namespace Fuzr.Core.Helpers;

public static class LogHelper
{
    public static void RegisterLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Warning)
            .CreateLogger();
    }
}