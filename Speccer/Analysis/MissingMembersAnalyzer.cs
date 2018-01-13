using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Speccer.Description;

namespace Speccer.Analysis
{
    public static class MissingMembersAnalyzer
    {
        public static (List<PropertyDescription> properties, List<FunctionDescription> functions)
            ResolveMissingMembers(IEnumerable<string> missingMembers, SyntaxTree tree, CSharpCompilation stubCompilation)
        {
            var memberNames = missingMembers.ToList();
            var tokens = SyntaxTreeUtils.NamesToTokens(tree, memberNames);

            // We've spotted that such a context information usually describes the usage,
            // like function invocation or member access.
            var parentsOfParentsOfParents = tokens.SelectMany(tokenOccurencies => tokenOccurencies.Select(namedToken => new Tuple<string, SyntaxNode>(namedToken.Item1, namedToken.Item2.Parent.Parent.Parent)));

            var semanticModel = stubCompilation.GetSemanticModel(tree);
            // Resolving named member context by node
            var descriptions =
                parentsOfParentsOfParents.Select(
                        namedNode => ResolveMemberType(namedNode.Item1, namedNode.Item2, semanticModel))
                    .ToList();
            //var descriptions = memberNames
            //    .Zip(parentsOfParentsOfParents,
            //        (name, node) => ResolveMemberType(name, node, semanticModel))
            //    .ToList();

            var propertiesFound = descriptions.OfType<PropertyDescription>().Combine().ToList();
            var functionsFound = descriptions.OfType<FunctionDescription>().Combine().ToList();

            return (propertiesFound, functionsFound);
        }

        private static object ResolveMemberType(string name, SyntaxNode node, SemanticModel semanticModel)
        {
            if (node is InvocationExpressionSyntax)
                return FunctionAnalyzer.ResolveFunctionInvocation(name, (InvocationExpressionSyntax)node, semanticModel);
            if (node is AssignmentExpressionSyntax)
                return PropertiesAnalyzer.ResolveSettableProperty(name, (AssignmentExpressionSyntax)node, semanticModel);
            return PropertiesAnalyzer.ResolveReadOnlyProperty(name, node, semanticModel);
        }

        public static List<string> GetMissingMembers(this CSharpCompilation compilation)
        {
            const string missingMemberErrorCode = "CS1061";
            return compilation
                .GetDiagnostics()
                .Where(diagnostic => diagnostic.Id == missingMemberErrorCode)
                .Select(CompilationUtils.GetNameFromDiagnostic)
                .Distinct()
                .ToList();
        }

    }
}
