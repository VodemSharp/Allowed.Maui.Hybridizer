using System.Text.Json;

namespace Allowed.Maui.Hybridizer.Abstractions.Plugins;

public record HwvPluginModuleResult(bool IsFound, JsonElement? Payload = null);