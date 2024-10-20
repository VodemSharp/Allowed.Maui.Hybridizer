using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Abstractions.Plugins;

namespace Allowed.Maui.Hybridizer.Essentials.Plugins;

[HwvPlugin]
public class AlertPlugin
{
    [HwvMethod]
    public void Show(Page page, [HwvPayload] ShowPayload payload)
    {
        page.Dispatcher.Dispatch(() =>
        {
            if (string.IsNullOrEmpty(payload.Accept) && payload.FlowDirection == null)
                page.DisplayAlert(payload.Title, payload.Message, payload.Cancel);
            else if (string.IsNullOrEmpty(payload.Accept) && payload.FlowDirection != null)
                page.DisplayAlert(payload.Title, payload.Message, payload.Cancel, payload.FlowDirection.Value);
            else if (!string.IsNullOrEmpty(payload.Accept) && payload.FlowDirection == null)
                page.DisplayAlert(payload.Title, payload.Message, payload.Accept, payload.Cancel);
            else
                page.DisplayAlert(payload.Title, payload.Message, payload.Accept, payload.Cancel,
                    payload.FlowDirection!.Value);
        });
    }

    public record ShowPayload(
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("message")]
        string Message,
        [property: JsonPropertyName("cancel")] string Cancel,
        [property: JsonPropertyName("accept")] string? Accept,
        [property: JsonPropertyName("flowDirection")]
        FlowDirection? FlowDirection);
}