using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Abstractions.Plugins;

namespace Allowed.Maui.Hybridizer.Essentials.Plugins;

[HwvPlugin]
public class SecureStoragePlugin
{
    [HwvMethod]
    public async Task<GetResponse> Get([HwvPayload] GetRequest body)
    {
        return new GetResponse(await SecureStorage.Default.GetAsync(body.Key));
    }

    [HwvMethod]
    public async Task Set([HwvPayload] SetRequest body)
    {
        await SecureStorage.Default.SetAsync(body.Key, body.Value);
    }

    [HwvMethod]
    public void Remove([HwvPayload] RemoveRequest body)
    {
        SecureStorage.Default.Remove(body.Key);
    }

    public record GetRequest([property: JsonPropertyName("key")] string Key);

    public record GetResponse([property: JsonPropertyName("value")] string? Value);

    public record SetRequest(
        [property: JsonPropertyName("key")] string Key,
        [property: JsonPropertyName("value")] string Value);

    public record RemoveRequest([property: JsonPropertyName("key")] string Key);
}