using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Speccer.Description;
using Speccer.Generation;

namespace Speccer.Analysis
{
    public class FixtureAnalyzer
    {
        public ClassDescription ExtractSpecification(string testFixture)
        {
            // STAGE 1 - RESOLVE CLASS & NAMESPACE NAME
            var tree = CSharpSyntaxTree.ParseText(testFixture);

            var namespaceName = NamespaceAnalyzer
                .ExtractFixtureNamespace(tree)
                .ToModuleNamespace();
            var className = FixtureNameAnalyzer
                .ExtractFixtureName(tree)
                .ToTargetClassName();

            // STAGE 2 - FIND MISSING MEMBERS
            var emptyClassStub = buildTemporaryStub(namespaceName, className);
            var stubTree = CSharpSyntaxTree.ParseText(emptyClassStub);

            var stubCompilation = CSharpCompilation.Create("TestedAssembly")
                .AddReferences(
                    MetadataReference.CreateFromFile(
                        typeof(object).Assembly.Location))
                .AddSyntaxTrees(stubTree)
                .AddSyntaxTrees(tree);

            var missingMembers = stubCompilation
                .GetDiagnostics()
                .Where(diagnostic => diagnostic.Id == "CS1061")
                .Select(GetNameFromDiagnostic)
                .Distinct()
                .ToList();


            // STAGE 3 - SYNTAX-BASED - FIND SPECIFICATION OF MISSING MEMBERS
            var tokens = NamesToTokens(tree, missingMembers);

            var parentsOfParentsOfParents = tokens.Select(token => token.Parent.Parent.Parent);

            var descriptions = missingMembers.Zip(parentsOfParentsOfParents, (name, node) =>
            {
                if (node is AssignmentExpressionSyntax)
                    return ResolveSettableProperty(name, (AssignmentExpressionSyntax)node);
                if (node is InvocationExpressionSyntax)
                    return ResolveFunctionInvocation(name, (InvocationExpressionSyntax)node);
                return ResolveReadOnlyProperty(name, node);
            }).ToList();
            var propertiesFound = descriptions.OfType<PropertyDescription>().ToList();
            var functionsFound = descriptions.OfType<FunctionDescription>().ToList();

            return new ClassDescription(className, namespaceName, propertiesFound, functionsFound);
        }

        private object ResolveSettableProperty(string propertyName, AssignmentExpressionSyntax node)
        {
            return new PropertyDescription(propertyName, "object", true);
        }

        private object ResolveFunctionInvocation(string functionName, InvocationExpressionSyntax node)
        {
            var returnType = "void";
            var varDeclaration = node.Ancestors().OfType<VariableDeclarationSyntax>().First();
            if (varDeclaration != null)
            {
                var predefinedType = varDeclaration.ChildNodes().OfType<PredefinedTypeSyntax>().First();
                returnType = predefinedType.Keyword.Value.ToString();
            }

            return new FunctionDescription(functionName, returnType, new string[] { });
        }

        private object ResolveReadOnlyProperty(string propertyName, SyntaxNode node)
        {
            return new PropertyDescription(propertyName, "object", false);
        }

        private string buildTemporaryStub(string namespaceName, string className)
        {
            var description = new ClassDescription(className, namespaceName, new PropertyDescription[]{}, new FunctionDescription[] { });
            var generator = new ClassGenerator(description);
            return generator.GenerateClass();
        }

        private string buildFunctionsOnlyStub(string namespaceName, string className,
            IEnumerable<FunctionDescription> dummyFunctions)
        {
            var description = new ClassDescription(className, namespaceName, new PropertyDescription[] { }, dummyFunctions);
            var generator = new ClassGenerator(description);
            return generator.GenerateClass();
        }

        private string GetNameFromDiagnostic(Diagnostic diagnostic)
        {
            var span = diagnostic.Location.SourceSpan;
            var sourceCode = diagnostic.Location.SourceTree.ToString();
            return sourceCode.Substring(span.Start, span.Length);
        }

        private IEnumerable<SyntaxToken> NamesToTokens(SyntaxTree tree, IEnumerable<string> memberNames)
        {
            return memberNames.Select(name => ((CompilationUnitSyntax)tree.GetRoot()).DescendantTokens().First(token => token.Value is string && (string)token.Value == name)).ToList();
        }
    }
}
