using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Abstractions.Plugins;

namespace Allowed.Maui.Hybridizer.Essentials.Plugins;

[HwvPlugin]
public class BatteryPlugin
{
    [HwvMethod]
    public GetInfoResponse GetInfo()
    {
        return new GetInfoResponse
        {
            State = Battery.Default.State,
            ChargeLevel = Battery.Default.ChargeLevel
        };
    }

    public class GetInfoResponse
    {
        [JsonPropertyName("state")] public BatteryState State { get; set; }
        [JsonPropertyName("chargeLevel")] public double ChargeLevel { get; set; }
    }
}