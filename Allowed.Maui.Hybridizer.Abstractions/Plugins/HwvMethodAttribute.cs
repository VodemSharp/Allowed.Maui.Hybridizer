namespace Allowed.Maui.Hybridizer.Abstractions.Plugins;

[AttributeUsage(AttributeTargets.Method)]
public class HwvMethodAttribute : Attribute
{
    public string? Name { get; set; }
}