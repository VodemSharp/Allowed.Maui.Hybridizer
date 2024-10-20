using System.Text.Json;
using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Abstractions.Plugins;
using Allowed.Maui.Hybridizer.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace Allowed.Maui.Hybridizer.Plugins;

public class HwvPluginHandler(IServiceProvider serviceProvider, HwvContext context, HwvPluginResponder responder)
{
    private HwvPluginModuleHandler[] _handlers = null!;

    public void Initialize(HwvPluginModuleHandler[] handlers)
    {
        _handlers = handlers;
    }

    public async Task HandleRawMessage(Page page, string message)
    {
        var request = JsonSerializer.Deserialize<HwvPluginRequest>(message);
        if (request == null) return;

        await using var scope = serviceProvider.CreateAsyncScope();

        var result = new HwvPluginModuleResult(false);

        foreach (var handler in _handlers)
        {
            result = await handler.Invoke(
                scope.ServiceProvider, context.Page, request.PluginName, request.MethodName, request.Payload);

            if (result.IsFound) break;
        }

        page.Dispatcher.Dispatch(() =>
        {
            if (result.Payload == null)
                responder.Reply(request.TaskId);
            else
                responder.Reply(request.TaskId, result.Payload);
        });
    }
}

public delegate Task<HwvPluginModuleResult> HwvPluginModuleHandler(
    IServiceProvider provider, Page page, string pluginName, string methodName, JsonElement? body);

public class HwvPluginRequest
{
    [JsonPropertyName("taskId")] public long TaskId { get; set; }
    [JsonPropertyName("plugin")] public string PluginName { get; set; } = null!;
    [JsonPropertyName("method")] public string MethodName { get; set; } = null!;
    [JsonPropertyName("payload")] public JsonElement? Payload { get; set; }
}