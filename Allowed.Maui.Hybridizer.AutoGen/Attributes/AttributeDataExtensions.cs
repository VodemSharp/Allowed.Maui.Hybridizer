using Microsoft.CodeAnalysis;

namespace Allowed.Maui.Hybridizer.AutoGen.Attributes;

public static class AttributeDataExtensions
{
    public static T? GetNamedArgument<T>(this AttributeData attributeData, string argumentName)
    {
        return (T?)attributeData.NamedArguments
            .Where(x => x.Key == argumentName)
            .Select(x => x.Value.Value)
            .SingleOrDefault();
    }
}