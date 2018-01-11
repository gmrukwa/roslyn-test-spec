using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Speccer.Analysis
{
    public static class FixtureNameAnalyzer
    {
        public static string ExtractFixtureName(SyntaxTree tree)
        {
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var fixture = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .First();
            var identifier = fixture
                .ChildTokens()
                .First(token => token.IsKind(SyntaxKind.IdentifierToken));
            return (string)identifier.Value;
        }

        public static string ToTargetClassName(this string fixtureNamespace)
        {
            return fixtureNamespace.Replace("Test", "");
        }
    }
}
