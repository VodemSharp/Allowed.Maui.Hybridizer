using Microsoft.CodeAnalysis;

namespace Allowed.Maui.Hybridizer.AutoGen.Diagnostics;

public static class DiagnosticHelper
{
    public static Diagnostic GetAttributeNotFound(string attributeName)
    {
        return Diagnostic.Create(
            new DiagnosticDescriptor(
                "AMHA001",
                "Attribute not found",
                $"Attribute '{attributeName}' could not be found.",
                "CodeGeneration",
                DiagnosticSeverity.Error,
                true),
            Location.None);
    }

    public static Diagnostic GetParameterMappingError(IParameterSymbol param)
    {
        return Diagnostic.Create(
            new DiagnosticDescriptor(
                "AMHA002",
                "Parameter mapping error",
                $"Cannot find a matching variable for parameter '{param.Name}' of type '{param.Type.ToDisplayString()}'.",
                "CodeGeneration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true),
            Location.None);
    }

    public static Diagnostic GetTypeNotFound(string typeName)
    {
        return Diagnostic.Create(
            new DiagnosticDescriptor(
                "HWV003",
                "Type not found",
                $"Type '{typeName}' could not be found.",
                "CodeGeneration",
                DiagnosticSeverity.Error,
                true),
            Location.None);
    }
}