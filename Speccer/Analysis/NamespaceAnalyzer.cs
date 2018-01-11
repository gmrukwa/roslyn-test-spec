using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Speccer.Analysis
{
    public static class NamespaceAnalyzer
    {
        public static NameSyntax ExtractFixtureNamespace(SyntaxTree tree)
        {
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var namespaceDeclaration = root.DescendantNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .First();
            return namespaceDeclaration.Name;
        }

        public static string ToModuleNamespace(this NameSyntax fixtureNamespace)
        {
            return fixtureNamespace.ToString().Replace(".Test", "");
        }
    }
}
