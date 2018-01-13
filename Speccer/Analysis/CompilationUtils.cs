using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Speccer.Analysis
{
    public static class CompilationUtils
    {
        public static List<PortableExecutableReference> GetAssemblyReferences()
        {
            return ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))
                .Split(Path.PathSeparator)
                .Where(path => path.EndsWith(".dll"))
                .Select(path => MetadataReference.CreateFromFile(path))
                .ToList();
        }

        public static CSharpCompilationOptions GetCompilationOptions() =>
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        public static CSharpCompilation Compile(this IEnumerable<SyntaxTree> trees)
        {
            return CSharpCompilation.Create(
                "TestedAssembly.dll",
                references: CompilationUtils.GetAssemblyReferences(),
                syntaxTrees: trees,
                options: CompilationUtils.GetCompilationOptions());
        }

        public static string GetNameFromDiagnostic(Diagnostic diagnostic)
        {
            var span = diagnostic.Location.SourceSpan;
            var sourceCode = diagnostic.Location.SourceTree.ToString();
            return sourceCode.Substring(span.Start, span.Length);
        }
    }
}
