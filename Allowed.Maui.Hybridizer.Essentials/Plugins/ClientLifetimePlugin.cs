using Allowed.Maui.Hybridizer.Abstractions.Plugins;

namespace Allowed.Maui.Hybridizer.Essentials.Plugins;

[HwvPlugin]
public class ClientLifetimePlugin(IServiceProvider serviceProvider, ClientLifetimeMethods methods)
{
    [HwvMethod]
    public void Initialize() => methods.OnInitialized?.Invoke(serviceProvider);

    [HwvMethod]
    public void Dispose() => methods.OnDisposed?.Invoke(serviceProvider);
}

public class ClientLifetimeMethods(
    Action<IServiceProvider>? onInitialized = null,
    Action<IServiceProvider>? onDisposed = null)
{
    public Action<IServiceProvider>? OnInitialized { get; set; } = onInitialized;
    public Action<IServiceProvider>? OnDisposed { get; set; } = onDisposed;
}