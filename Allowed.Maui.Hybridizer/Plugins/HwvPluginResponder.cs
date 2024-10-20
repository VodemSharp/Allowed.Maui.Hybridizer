using System.Text.Json;
using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Contexts;
using Allowed.Maui.Hybridizer.Messages;

namespace Allowed.Maui.Hybridizer.Plugins;

public class HwvPluginResponder(HwvContext context)
{
    public void Reply(long taskId)
    {
        context.HybridWebView.SendRawMessage(
            AddPluginPrefix(JsonSerializer.Serialize(new HwvResponse { TaskId = taskId })));
    }

    public void Reply<T>(long taskId, T payload)
    {
        context.HybridWebView.SendRawMessage(
            AddPluginPrefix(JsonSerializer.Serialize(new HwvResponse<T> { TaskId = taskId, Payload = payload })));
    }

    private static string AddPluginPrefix(string response)
    {
        return $"{HwvMessageTypes.Plugin}|{response}";
    }
}

public class HwvResponse
{
    [JsonPropertyName("taskId")] public long TaskId { get; set; }
}

public class HwvResponse<T> : HwvResponse
{
    [JsonPropertyName("payload")] public T? Payload { get; set; }
}