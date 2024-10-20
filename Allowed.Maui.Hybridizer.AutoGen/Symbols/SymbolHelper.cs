using Allowed.Maui.Hybridizer.AutoGen.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Allowed.Maui.Hybridizer.AutoGen.Symbols;

public class SymbolHelper
{
    private static readonly SymbolEqualityComparer Comparer = SymbolEqualityComparer.Default;
    private readonly INamedTypeSymbol _taskOfTSymbol;

    private readonly INamedTypeSymbol _taskSymbol;
    private readonly INamedTypeSymbol _valueTaskOfTSymbol;
    private readonly INamedTypeSymbol _valueTaskSymbol;

    public SymbolHelper(Compilation compilation)
    {
        _taskSymbol = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task")!;
        _taskOfTSymbol = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1")!;
        _valueTaskSymbol = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask")!;
        _valueTaskOfTSymbol = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask`1")!;
    }

    public static ITypeSymbol? GetTypeByMetadataName(Compilation compilation, string metadataName,
        SourceProductionContext context)
    {
        var typeSymbol = compilation.GetTypeByMetadataName(metadataName);
        if (typeSymbol == null)
            context.ReportDiagnostic(DiagnosticHelper.GetTypeNotFound(metadataName));

        return typeSymbol;
    }

    public static AttributeData? GetAttribute(ISymbol symbol, INamedTypeSymbol hwvPluginAttributeSymbol)
    {
        return symbol.GetAttributes()
            .SingleOrDefault(attr => Comparer.Equals(attr.AttributeClass, hwvPluginAttributeSymbol));
    }

    public bool IsTaskMethod(IMethodSymbol methodSymbol)
    {
        var returnType = methodSymbol.ReturnType;

        return Comparer.Equals(returnType, _taskSymbol) ||
               Comparer.Equals(returnType, _valueTaskSymbol);
    }

    public bool IsTaskOfMethod(IMethodSymbol methodSymbol)
    {
        var returnType = methodSymbol.ReturnType;

        return Comparer.Equals(returnType.OriginalDefinition, _taskOfTSymbol) ||
               Comparer.Equals(returnType.OriginalDefinition, _valueTaskOfTSymbol);
    }
}