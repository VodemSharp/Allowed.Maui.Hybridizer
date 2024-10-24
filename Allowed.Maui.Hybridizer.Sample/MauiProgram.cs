﻿using Allowed.Maui.Hybridizer.Essentials;
using Allowed.Maui.Hybridizer.Sample.Plugins;
using Allowed.Maui.Hybridizer.Sample.Services;
using Microsoft.Extensions.Logging;
using HwvPluginModule = Allowed.Maui.Hybridizer.Essentials.HwvPluginModule;

namespace Allowed.Maui.Hybridizer.Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddHybridizer(HwvPluginModule.Invoke, Plugins.HwvPluginModule.Invoke);
        builder.Services.AddSingleton<AppLifecycleService>();

        builder.Services.RegisterAlertPlugin();
        builder.Services.RegisterBatteryPlugin();
        builder.Services.RegisterStoragePlugin();

        builder.Services.RegisterCustomAlertPlugin();

        builder.Services.RegisterClientLifetimePlugin(scope =>
        {
            var lifecycleService = scope.GetService<AppLifecycleService>();
            lifecycleService?.OnHwvInitialized();
        }, scope =>
        {
            var lifecycleService = scope.GetService<AppLifecycleService>();
            lifecycleService?.OnHwvDisposed();
        });

        return builder.Build();
    }
}