using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Callers;
using Allowed.Maui.Hybridizer.Contexts;
using Allowed.Maui.Hybridizer.Messages;
using Allowed.Maui.Hybridizer.Plugins;
using Allowed.Maui.Hybridizer.Sample.Services;
using Microsoft.Extensions.Logging;

namespace Allowed.Maui.Hybridizer.Sample;

public partial class MainPage
{
    private readonly HwvJsCaller _jsCaller;
    private readonly ILogger<MainPage> _logger;
    private readonly HwvPluginHandler _pluginHandler;

    public MainPage(HwvContext context, HwvPluginHandler pluginHandler, HwvJsCaller jsCaller,
        AppLifecycleService appLifecycle, ILogger<MainPage> logger)
    {
        _logger = logger;
        _pluginHandler = pluginHandler;
        _jsCaller = jsCaller;

        InitializeComponent();

        context.Initialize(this, HybridWebView);

        appLifecycle.Resumed += async () => { await _jsCaller.WrappedCall("Resumed"); };
        appLifecycle.Stopped += async () => { await _jsCaller.WrappedCall("Stopped"); };

        appLifecycle.HwvInitialized += async () =>
        {
            var webViewInfo = await _jsCaller.WrappedCall<GetWebViewInfo>("GetWebViewInfo");
            // actions with web view info...

            Battery.Default.BatteryInfoChanged += BatteryInfoChanged;
        };

        appLifecycle.HwvDisposed += () => { Battery.Default.BatteryInfoChanged -= BatteryInfoChanged; };
    }

    private async void BatteryInfoChanged(object? _, BatteryInfoChangedEventArgs args)
    {
        await _jsCaller.Call("BatteryInfoChanged", new { state = args.State, chargeLevel = args.ChargeLevel });
    }

    private async void OnHybridWebViewRawMessageReceived(object sender, HybridWebViewRawMessageReceivedEventArgs ev)
    {
        try
        {
            if (string.IsNullOrEmpty(ev.Message)) return;

            var messageParts = ev.Message.Split(['|'], 2);
            var messageType = messageParts[0];
            var message = messageParts.Length > 1 ? messageParts[1] : string.Empty;

            switch (messageType)
            {
                case HwvMessageTypes.Plugin:
                    await _pluginHandler.HandleRawMessage(this, message);
                    break;
                case HwvMessageTypes.Call:
                    _jsCaller.SetTaskResult(message);
                    break;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("{e}", e.ToString());
        }
    }
}

public class GetWebViewInfo
{
    [JsonPropertyName("userAgent")] public string UserAgent { get; set; } = null!;
}