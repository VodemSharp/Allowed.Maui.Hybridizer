using Allowed.Maui.Hybridizer.Sample.Services;

namespace Allowed.Maui.Hybridizer.Sample;

public partial class App : Application
{
    private readonly AppLifecycleService _appLifecycle;

    public App(AppLifecycleService appLifecycle)
    {
        _appLifecycle = appLifecycle;
        InitializeComponent();
    }
    
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());
        window.Resumed += _appLifecycle.OnResumed;
        window.Stopped += _appLifecycle.OnStopped;
        return window;
    }
}