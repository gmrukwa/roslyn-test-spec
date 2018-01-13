using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Speccer.Description;

namespace Speccer.Analysis
{
    class FunctionAnalyzer
    {
        public static object ResolveFunctionInvocation(string functionName, InvocationExpressionSyntax node, SemanticModel semanticModel)
        {
            var returnType = "void";
            var varDeclaration = node.Ancestors().OfType<VariableDeclarationSyntax>().FirstOrDefault();
            if (varDeclaration != null)
            {
                var predefinedType = varDeclaration.ChildNodes().OfType<PredefinedTypeSyntax>().First();
                returnType = predefinedType.Keyword.Value.ToString();
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
    }
}
