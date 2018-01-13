using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Speccer.Description;

namespace Speccer.Analysis
{
    public static class FunctionAnalyzer
    {
        public static object ResolveFunctionInvocation(string functionName, InvocationExpressionSyntax node, SemanticModel semanticModel)
        {
            var returnType = "void";
            var varDeclaration = node.Ancestors().OfType<VariableDeclarationSyntax>().FirstOrDefault();
            if (varDeclaration != null)
            {
                var predefinedType = varDeclaration.ChildNodes().OfType<PredefinedTypeSyntax>().FirstOrDefault();
                returnType = predefinedType?.Keyword.Value.ToString() ?? "object";
            }
            var arguments = GetArgumentTypes(node.ArgumentList.Arguments, semanticModel);

            return new FunctionDescription(functionName, returnType, arguments);
        }

        private static List<string> GetArgumentTypes(SeparatedSyntaxList<ArgumentSyntax> arguments, SemanticModel model)
        {
            return arguments
                .Select(argument => model.GetTypeInfo(argument.Expression).Type?.ToString() ?? "object")
                .Select(typeName => typeName == "?" ? "object" : typeName)
                .ToList();
        }

        public static List<FunctionDescription> Combine(this IEnumerable<FunctionDescription> functionsInfo)
        {
            var functions = functionsInfo.ToList();
            var names = functions.Select(function => function.Name).Distinct().ToList();
            return names.Select(functionName =>
            {
                var allInfoAboutFunction = functions.Where(function => function.Name == functionName).ToList();

                var recognizedOutputTypes = allInfoAboutFunction.Select(info => info.ReturnType);
                var returnType = recognizedOutputTypes.FirstOrDefault(type => type != "object") ?? "object";

                var recognizedArgumentTypes = allInfoAboutFunction.Select(info => info.Arguments.ToList()).ToList();

                var specializedTypes = Enumerable
                    .Range(0, recognizedArgumentTypes[0].Count)
                    .Select(i => recognizedArgumentTypes.Select(types => types[i]))
                    .Select(singleArgumentTypes => singleArgumentTypes.FirstOrDefault(type => type != "object") ??
                                                   "object");
                
                return new FunctionDescription(functionName, returnType, specializedTypes);
            }).ToList();
        }
    }
}
