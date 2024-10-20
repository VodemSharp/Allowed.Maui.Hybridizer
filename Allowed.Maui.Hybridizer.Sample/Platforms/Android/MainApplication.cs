using Android.App;
using Android.Runtime;

[assembly: UsesPermission(Android.Manifest.Permission.BatteryStats)]

namespace Allowed.Maui.Hybridizer.Sample;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}