using Allowed.Maui.Hybridizer.AutoGen.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Allowed.Maui.Hybridizer.AutoGen.Attributes;

public static class AttributeSymbolHelper
{
    public static INamedTypeSymbol? GetHwvPluginAttributeSymbol(
        SourceProductionContext context, Compilation compilation)
    {
        var hwvPluginAttributeSymbol = compilation.GetTypeByMetadataName(AttributeMetadataNames.HwvPluginAttribute);
        if (hwvPluginAttributeSymbol == null)
            context.ReportDiagnostic(DiagnosticHelper.GetAttributeNotFound(AttributeMetadataNames.HwvPluginAttribute));

        return hwvPluginAttributeSymbol;
    }

    public static INamedTypeSymbol? GetHwvMethodAttributeSymbol(
        SourceProductionContext context, Compilation compilation)
    {
        var hwvMethodAttributeSymbol = compilation.GetTypeByMetadataName(AttributeMetadataNames.HwvMethodAttribute);
        if (hwvMethodAttributeSymbol == null)
            context.ReportDiagnostic(DiagnosticHelper.GetAttributeNotFound(AttributeMetadataNames.HwvMethodAttribute));

        return hwvMethodAttributeSymbol;
    }
    
    public static INamedTypeSymbol? GetHwvPayloadAttributeSymbol(
        SourceProductionContext context, Compilation compilation)
    {
        var hwvRequestAttributeSymbol = compilation.GetTypeByMetadataName(AttributeMetadataNames.HwvPayloadAttribute);
        if (hwvRequestAttributeSymbol == null)
            context.ReportDiagnostic(DiagnosticHelper.GetAttributeNotFound(AttributeMetadataNames.HwvPayloadAttribute));

        return hwvRequestAttributeSymbol;
    }
}