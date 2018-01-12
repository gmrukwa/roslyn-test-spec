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
            var tree = CSharpSyntaxTree.ParseText(testFixture);


            var namespaceName = NamespaceAnalyzer
                .ExtractFixtureNamespace(tree)
                .ToModuleNamespace();
            var className = FixtureNameAnalyzer
                .ExtractFixtureName(tree)
                .ToTargetClassName();


            var emptyClassStub = buildTemporaryStub(namespaceName, className);
            var stubTree = CSharpSyntaxTree.ParseText(emptyClassStub);

            var compilation = CSharpCompilation.Create("TestedAssembly")
                .AddReferences(
                    MetadataReference.CreateFromFile(
                        typeof(object).Assembly.Location))
                .AddSyntaxTrees(stubTree)
                .AddSyntaxTrees(tree);

            var missingMembers = compilation
                .GetDiagnostics()
                .Where(diagnostic => diagnostic.Id == "CS1061")
                .Select(GetNameFromDiagnostic)
                .Distinct()
                .ToList();

            var functions = tree.GetFunctionCallsOn(compilation, className);

            return new ClassDescription(name: className,
                namesp: namespaceName, properties: new PropertyDescription[]{},
                functions: functions);
        }

        private string buildTemporaryStub(string namespaceName, string className)
        {
            var description = new ClassDescription(className, namespaceName, new PropertyDescription[]{}, new FunctionDescription[] { });
            var generator = new ClassGenerator(description);
            return generator.GenerateClass();
        }

        private string GetNameFromDiagnostic(Diagnostic diagnostic)
        {
            var span = diagnostic.Location.SourceSpan;
            var sourceCode = diagnostic.Location.SourceTree.ToString();
            return sourceCode.Substring(span.Start, span.Length);
        }
    }
}
