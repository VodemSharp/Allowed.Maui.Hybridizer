using Allowed.Maui.Hybridizer.Sample.Plugins.Core;

namespace Allowed.Maui.Hybridizer.Sample.Plugins;

public static class ServicesExtensions
{
    public static IServiceCollection RegisterCustomAlertPlugin(this IServiceCollection services)
    {
        return services.AddTransient<CustomAlertPlugin>();
    }
}