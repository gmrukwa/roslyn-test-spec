using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Speccer.Description;

namespace Speccer.Analysis
{
    public class FixtureAnalyzer
    {
        public ClassDescription ExtractSpecification(string testFixture)
        {
            var tree = CSharpSyntaxTree.ParseText(testFixture);
            var compilation = CSharpCompilation.Create("TestedAssembly")
                .AddReferences(
                    MetadataReference.CreateFromFile(
                        typeof(object).Assembly.Location))
                .AddSyntaxTrees(tree);
            var model = compilation.GetSemanticModel(tree);

            var namespaceName = NamespaceAnalyzer
                .ExtractFixtureNamespace(tree)
                .ToModuleNamespace();
            var className = FixtureNameAnalyzer
                .ExtractFixtureName(tree)
                .ToTargetClassName();

            var specification = new ClassDescription(name: className,
                namesp: namespaceName, properties: new PropertyDescription[]{},
                functions: new FunctionDescription[]{});

            return specification;
        }
    }
}
