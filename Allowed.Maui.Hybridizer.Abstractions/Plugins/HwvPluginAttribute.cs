namespace Allowed.Maui.Hybridizer.Abstractions.Plugins;

[AttributeUsage(AttributeTargets.Class)]
public class HwvPluginAttribute : Attribute
{
    public string? Name { get; set; }
}