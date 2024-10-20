using System.Collections.Immutable;
using System.Text;
using Allowed.Maui.Hybridizer.AutoGen.Attributes;
using Allowed.Maui.Hybridizer.AutoGen.Diagnostics;
using Allowed.Maui.Hybridizer.AutoGen.Symbols;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Allowed.Maui.Hybridizer.AutoGen.Generators;

[Generator(LanguageNames.CSharp)]
public class HwvPluginModuleGenerator : IIncrementalGenerator
{
    private const string PluginPart = "Plugin";
    private const string NameAttributeName = "Name";
    private const string DeserializedPayload = "deserializedPayload";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var pluginClasses = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                AttributeMetadataNames.HwvPluginAttribute,
                static (s, _) => s is ClassDeclarationSyntax,
                static (context, _) => (ClassDeclarationSyntax)context.TargetNode)
            .Collect();

        var compilationAndClasses = context.CompilationProvider.Combine(pluginClasses);
        context.RegisterSourceOutput(compilationAndClasses, GenerateSource);
    }

    private static void GenerateSource(SourceProductionContext context,
        (Compilation, ImmutableArray<ClassDeclarationSyntax>) source)
    {
        var (compilation, classes) = source;

        var hwvPluginAttributeSymbol = AttributeSymbolHelper.GetHwvPluginAttributeSymbol(context, compilation);
        if (hwvPluginAttributeSymbol == null)
            return;

        var hwvMethodAttributeSymbol = AttributeSymbolHelper.GetHwvMethodAttributeSymbol(context, compilation);
        if (hwvMethodAttributeSymbol == null)
            return;

        var plugins = ImmutableList.CreateBuilder<HwvPlugin>();

        foreach (var classDeclaration in classes.Distinct())
        {
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            if (ModelExtensions.GetDeclaredSymbol(semanticModel, classDeclaration) is not INamedTypeSymbol classSymbol)
                continue;

            var hwvPluginAttributeInstance = SymbolHelper.GetAttribute(classSymbol, hwvPluginAttributeSymbol);
            if (hwvPluginAttributeInstance == null)
                continue;

            var pluginAlias = hwvPluginAttributeInstance.GetNamedArgument<string>(NameAttributeName);

            if (pluginAlias == null)
            {
                pluginAlias = classSymbol.Name;
                pluginAlias = pluginAlias.EndsWith(PluginPart)
                    ? pluginAlias.Substring(0, pluginAlias.Length - PluginPart.Length)
                    : pluginAlias;
            }

            var hwvPlugin = new HwvPlugin(pluginAlias, classSymbol);

            foreach (var methodSymbol in classSymbol.GetMembers().OfType<IMethodSymbol>())
            {
                var hwvMethodAttributeInstance = SymbolHelper.GetAttribute(methodSymbol, hwvMethodAttributeSymbol);
                if (hwvMethodAttributeInstance == null) continue;

                var methodName = methodSymbol.Name;
                var methodAlias =
                    hwvMethodAttributeInstance.GetNamedArgument<string>(NameAttributeName) ?? methodName;

                hwvPlugin.Methods.Add(new HwvMethod(methodName, methodAlias, methodSymbol));
            }

            plugins.Add(hwvPlugin);
        }

        GenerateHwvPluginModule(context, compilation, plugins.ToImmutable());
    }

    private static void GenerateHwvPluginModule(
        SourceProductionContext context, Compilation compilation, ImmutableList<HwvPlugin> plugins)
    {
        var namespacesBuilder = new StringBuilder();
        var moduleDeclarationBuilder = new StringBuilder();
        var moduleBodyBuilder = new StringBuilder();
        var moduleEndingBuilder = new StringBuilder();

        var symbolHelper = new SymbolHelper(compilation);

        var payloadAttributeSymbol = AttributeSymbolHelper.GetHwvPayloadAttributeSymbol(context, compilation);
        if (payloadAttributeSymbol == null)
            return;

        namespacesBuilder.AppendLine(
            """
            using Allowed.Maui.Hybridizer.Abstractions.Plugins;
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Maui.Controls;
            using System;
            using System.Text.Json;
            using System.Threading.Tasks;
            """);
        
        var typeToVariable = new Dictionary<string, string>
        {
            { "Microsoft.Maui.Controls.Page", "page" }
        };
        
        var availableParameters = new Dictionary<ITypeSymbol, string>(SymbolEqualityComparer.Default);
        foreach (var ttv in typeToVariable)
        {
            var typeSymbol = SymbolHelper.GetTypeByMetadataName(compilation, ttv.Key, context);
            if (typeSymbol == null)
                return;
            
            availableParameters.Add(typeSymbol, ttv.Value);
        }

        moduleBodyBuilder.AppendLine(
            """
            switch (pluginName)
            {
            """);

        foreach (var plugin in plugins)
        {
            var pluginTypeName = plugin.PluginSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            moduleBodyBuilder.AppendLine(
                $$"""
                  case "{{plugin.PluginAlias}}":
                  {
                      var plugin = provider.GetRequiredService<{{pluginTypeName}}>();
                  
                      switch (methodName)
                      {
                  """);

            foreach (var method in plugin.Methods)
            {
                var methodParameters = method.MethodSymbol.Parameters;

                var argumentList = new List<string>();
                var preInvocationCode = new StringBuilder();

                foreach (var param in methodParameters)
                {
                    var payloadAttributeInstance = SymbolHelper.GetAttribute(param, payloadAttributeSymbol);

                    if (payloadAttributeInstance != null)
                    {
                        var paramTypeName = param.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                        preInvocationCode.AppendLine(
                            $"var {DeserializedPayload} = payload.HasValue ? payload.Value.Deserialize<{paramTypeName}>() : default;");
                        argumentList.Add(DeserializedPayload);
                    }
                    else
                    {
                        if (!availableParameters.TryGetValue(param.Type, out var variable))
                        {
                            context.ReportDiagnostic(DiagnosticHelper.GetParameterMappingError(param));
                            return;
                        }

                        argumentList.Add(variable);
                    }
                }

                var arguments = string.Join(", ", argumentList);

                var isTaskMethod = symbolHelper.IsTaskMethod(method.MethodSymbol);
                var isTaskOfMethod = symbolHelper.IsTaskOfMethod(method.MethodSymbol);
                var isAwaitable = isTaskMethod || isTaskOfMethod;
                var hasReturnType = isTaskOfMethod || !isTaskMethod && !method.MethodSymbol.ReturnsVoid;

                var methodInvocation =
                    $"{(hasReturnType ? "var result = " : "")}{(isAwaitable ? "await " : "")}plugin.{method.MethodName}({arguments});";

                var payloadCode = hasReturnType
                    ? """
                      var serializedResult = JsonSerializer.SerializeToElement(result);
                      return new HwvPluginModuleResult(true, serializedResult);
                      """
                    : "return new HwvPluginModuleResult(true);";

                moduleBodyBuilder.AppendLine(
                    $$"""
                      case "{{method.MethodAlias}}":
                      {
                          {{preInvocationCode}}
                          {{methodInvocation}}
                          {{payloadCode}}
                      }
                      """);
            }

            moduleBodyBuilder.AppendLine(
                """
                        default:
                            throw new Exception($"Unknown method '{methodName}' for plugin '{pluginName}'");
                    }
                }
                """);
        }

        moduleBodyBuilder.AppendLine(
            """
                default:
                    return new HwvPluginModuleResult(false);
            }
            """);

        moduleDeclarationBuilder.AppendLine(
            $$"""
              namespace {{compilation.AssemblyName}};

              public static class HwvPluginModule
              {
                  public static async Task<HwvPluginModuleResult> Invoke(IServiceProvider provider, Page page, string pluginName, string methodName, JsonElement? payload)
                  {
              """);

        moduleEndingBuilder.AppendLine(
            """
                }
            }
            """);

        var resultBuilder = new StringBuilder();
        resultBuilder.Append(namespacesBuilder);
        resultBuilder.Append(moduleDeclarationBuilder);
        resultBuilder.Append(moduleBodyBuilder);
        resultBuilder.Append(moduleEndingBuilder);

        context.AddSource("HwvPluginModule.g.cs", FormatCode(resultBuilder.ToString()));
    }

    private static SourceText FormatCode(string sourceCode)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var formattedRoot = syntaxTree.GetRoot().NormalizeWhitespace();
        return SourceText.From(formattedRoot.ToFullString(), Encoding.UTF8);
    }
    
    public class HwvPlugin(string pluginAlias, INamedTypeSymbol pluginSymbol)
    {
        public string PluginAlias { get; set; } = pluginAlias;
        public INamedTypeSymbol PluginSymbol { get; set; } = pluginSymbol;
        public List<HwvMethod> Methods { get; set; } = [];
    }
    
    public class HwvMethod(string methodName, string methodAlias, IMethodSymbol methodSymbol)
    {
        public string MethodName { get; set; } = methodName;
        public string MethodAlias { get; set; } = methodAlias;
        public IMethodSymbol MethodSymbol { get; set; } = methodSymbol;
    }
}