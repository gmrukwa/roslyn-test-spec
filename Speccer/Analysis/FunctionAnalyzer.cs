﻿using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Speccer.Description;

namespace Speccer.Analysis
{
    class FunctionAnalyzer
    {
        public static object ResolveFunctionInvocation(string functionName, InvocationExpressionSyntax node)
        {
            var returnType = "void";
            var varDeclaration = node.Ancestors().OfType<VariableDeclarationSyntax>().FirstOrDefault();
            if (varDeclaration != null)
            {
                var predefinedType = varDeclaration.ChildNodes().OfType<PredefinedTypeSyntax>().First();
                returnType = predefinedType.Keyword.Value.ToString();
            }

            var numberOfArguments = GetNumberOfArguments(node);
            var arguments = new [] {"object"}.Cycle().Take(numberOfArguments);

            return new FunctionDescription(functionName, returnType, arguments);
        }

        public static int GetNumberOfArguments(InvocationExpressionSyntax node) => node.ArgumentList.Arguments.Count;
    }
}
