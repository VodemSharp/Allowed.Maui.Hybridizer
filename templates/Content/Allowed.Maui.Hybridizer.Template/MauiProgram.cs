using Allowed.Maui.Hybridizer;
using Allowed.Maui.Hybridizer.Essentials;
using Microsoft.Extensions.Logging;

namespace Allowed.Maui.Hybridizer.Template;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
//-:cnd:noEmit
#if DEBUG
        builder.Logging.AddDebug();
#endif
//+:cnd:noEmit

        builder.Services.AddHybridizer(HwvPluginModule.Invoke);
        builder.Services.RegisterAlertPlugin();

        return builder.Build();
    }
}