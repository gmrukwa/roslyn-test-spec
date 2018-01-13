using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Speccer.Analysis
{
    public class SyntaxTreeUtils
    {
        public static List<List<Tuple<string,SyntaxToken>>> NamesToTokens(SyntaxTree tree, IEnumerable<string> memberNames)
        {
            return memberNames
                .Select(name => ((CompilationUnitSyntax)tree.GetRoot())
                    .DescendantTokens()
                    .Where(token => token.Value is string && (string)token.Value == name)
                    .Select(token => new Tuple<string, SyntaxToken>(name, token))
                    .ToList())
                .ToList();
        }
    }
}
