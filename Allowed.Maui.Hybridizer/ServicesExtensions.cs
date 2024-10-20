using Allowed.Maui.Hybridizer.Abstractions.Callers;
using Allowed.Maui.Hybridizer.Abstractions.Plugins;
using Allowed.Maui.Hybridizer.Callers;
using Allowed.Maui.Hybridizer.Contexts;
using Allowed.Maui.Hybridizer.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Allowed.Maui.Hybridizer;

public static class ServicesExtensions
{
    public static IServiceCollection AddHybridizer(this IServiceCollection services,
        params HwvPluginModuleHandler[] handlers)
    {
        services.AddSingleton<HwvContext>();
        services.AddSingleton<HwvPluginResponder>();

        services.AddSingleton<HwvJsCaller>();
        services.AddSingleton<IHwvJsCaller>(provider => provider.GetRequiredService<HwvJsCaller>());

        services.AddSingleton<HwvPluginHandler>(provider =>
        {
            var pluginHandler = new HwvPluginHandler(provider,
                provider.GetRequiredService<HwvContext>(),
                provider.GetRequiredService<HwvPluginResponder>());
            
            pluginHandler.Initialize(handlers);
            return pluginHandler;
        });

        return services;
    }
}