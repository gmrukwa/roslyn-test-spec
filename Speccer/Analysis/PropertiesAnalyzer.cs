using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Speccer.Description;

namespace Speccer.Analysis
{
    public static class PropertiesAnalyzer
    {
        public static object ResolveSettableProperty(string propertyName, AssignmentExpressionSyntax node, SemanticModel semanticModel)
        {
            var returnType = "object";
            var varDeclaration = node.Ancestors().OfType<ExpressionStatementSyntax>().FirstOrDefault();
            if (varDeclaration != null)
            {
                var predefinedType = varDeclaration.ChildNodes().OfType<AssignmentExpressionSyntax>().First();
                returnType = semanticModel.GetTypeInfo(predefinedType.Right).Type.ToString();
            }

            return new PropertyDescription(propertyName, returnType, true);
        }

        public static object ResolveReadOnlyProperty(string propertyName, SyntaxNode node, SemanticModel semanticModel)
        {
            var returnType = "object";
            var varDeclaration = node.Ancestors().OfType<ExpressionStatementSyntax>().FirstOrDefault();
            if (varDeclaration != null)
            {
                var predefinedType = varDeclaration.ChildNodes().OfType<AssignmentExpressionSyntax>().First();
                returnType = semanticModel.GetTypeInfo(predefinedType.Right).Type.ToString();
            }

            return new PropertyDescription(propertyName, returnType, false);
        }
    }
}
