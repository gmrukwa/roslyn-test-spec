using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Speccer.Analysis
{
    public class SyntaxTreeUtils
    {
        public static IEnumerable<SyntaxToken> NamesToTokens(SyntaxTree tree, IEnumerable<string> memberNames)
        {
            return memberNames
                .Select(name => ((CompilationUnitSyntax)tree.GetRoot())
                    .DescendantTokens()
                    .First(token => token.Value is string && (string)token.Value == name))
                .ToList();
        }
    }
}
