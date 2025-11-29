using System.Diagnostics;
using System.Reflection;

namespace BT.Common.Services.Concrete;

public static class TelemetryHelperService
{
    public static ActivitySource ActivitySource = new (Assembly.GetAssembly(typeof(TelemetryHelperService))?.GetName().Name ?? string.Empty);

    internal static void SetActivitySource(ActivitySource activitySource)
    {
        ActivitySource = activitySource;
    }
}