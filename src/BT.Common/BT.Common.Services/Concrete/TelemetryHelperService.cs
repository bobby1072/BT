using System.Diagnostics;
using System.Reflection;

namespace BT.Common.Services.Concrete;

public static class TelemetryHelperService
{
    private static ActivitySource _internalActivitySource = new (Assembly.GetAssembly(typeof(TelemetryHelperService))?.GetName().Name ?? string.Empty);
    public static ActivitySource ActivitySource => _internalActivitySource;
    internal static void SetActivitySource(ActivitySource activitySource)
    {
        _internalActivitySource = activitySource;
    }
}