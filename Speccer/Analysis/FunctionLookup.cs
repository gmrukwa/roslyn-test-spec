using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Speccer.Description;

namespace Speccer.Analysis
{
    public static class FunctionLookup
    {
        public static List<FunctionDescription> GetFunctionCallsOn(this SyntaxTree tree, Compilation compilation,
            string className)
        {
            return new FunctionDescription[] { }.ToList();
        }
    }
}
