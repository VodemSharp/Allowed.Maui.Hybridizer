using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Abstractions.Plugins;
using Microsoft.Maui.Storage;

namespace Allowed.Maui.Hybridizer.Debugger;

[HwvPlugin]
public class StoragePlugin
{
    [HwvMethod]
    public GetResponse Get([HwvPayload] GetRequest request)
    {
        return new GetResponse(Preferences.Default.Get(request.Key, string.Empty));
    }

    [HwvMethod]
    public void Set([HwvPayload] SetRequest request)
    {
        Preferences.Default.Set(request.Key, request.Value);
    }

    [HwvMethod]
    public void Remove([HwvPayload] RemoveRequest request)
    {
        Preferences.Default.Remove(request.Key);
    }

    public record GetRequest([property: JsonPropertyName("key")] string Key);

    public record GetResponse([property: JsonPropertyName("value")] string Value);

    public record SetRequest(
        [property: JsonPropertyName("key")] string Key,
        [property: JsonPropertyName("value")] string Value);

    public record RemoveRequest([property: JsonPropertyName("key")] string Key);
}