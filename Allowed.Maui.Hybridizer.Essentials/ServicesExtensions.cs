using Allowed.Maui.Hybridizer.Essentials.Plugins;

namespace Allowed.Maui.Hybridizer.Essentials;

public static class ServicesExtensions
{
    public static IServiceCollection RegisterAlertPlugin(this IServiceCollection services)
    {
        return services.AddTransient<AlertPlugin>();
    }

    public static IServiceCollection RegisterBatteryPlugin(this IServiceCollection services)
    {
        return services.AddTransient<BatteryPlugin>();
    }
    
    public static IServiceCollection RegisterClientLifetimePlugin(this IServiceCollection services, 
        Action<IServiceProvider> onInitialized, Action<IServiceProvider> onDisposed)
    {
        services.AddTransient<ClientLifetimeMethods>(_ => new ClientLifetimeMethods
        {
            OnInitialized = onInitialized,
            OnDisposed = onDisposed
        });
        
        return services.AddTransient<ClientLifetimePlugin>();
    }

    public static IServiceCollection RegisterSecureStoragePlugin(this IServiceCollection services)
    {
        return services.AddTransient<SecureStoragePlugin>();
    }


    public static IServiceCollection RegisterStoragePlugin(this IServiceCollection services)
    {
        return services.AddTransient<StoragePlugin>();
    }
}