using Allowed.Maui.Hybridizer.Callers;
using Allowed.Maui.Hybridizer.Contexts;
using Allowed.Maui.Hybridizer.Messages;
using Allowed.Maui.Hybridizer.Plugins;
using Microsoft.Extensions.Logging;

namespace Allowed.Maui.Hybridizer.Template;

public partial class MainPage
{
    private readonly HwvJsCaller _jsCaller;
    private readonly ILogger<MainPage> _logger;
    private readonly HwvPluginHandler _pluginHandler;

    public MainPage(HwvContext context, HwvJsCaller jsCaller, ILogger<MainPage> logger, HwvPluginHandler pluginHandler)
    {
        _jsCaller = jsCaller;
        _logger = logger;
        _pluginHandler = pluginHandler;

        InitializeComponent();

        context.Initialize(this, HybridWebView);
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