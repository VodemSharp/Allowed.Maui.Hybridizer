namespace Allowed.Maui.Hybridizer.Sample.Services;

public class AppLifecycleService
{
    public event Action? Resumed;
    public event Action? Stopped;

    public event Action? HwvInitialized;
    public event Action? HwvDisposed;
    
    public void OnResumed(object? sender, EventArgs args) => Resumed?.Invoke();
    public void OnStopped(object? sender, EventArgs args) => Stopped?.Invoke();
    
    public void OnHwvInitialized() => HwvInitialized?.Invoke();
    public void OnHwvDisposed() => HwvDisposed?.Invoke();
}