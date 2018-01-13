using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Speccer.Description;
using Speccer.Generation;

namespace Speccer.Analysis
{
    public static class FixtureAnalyzer
    {
        public static ClassDescription ExtractSpecification(string testFixture)
        {
            var tree = CSharpSyntaxTree.ParseText(testFixture);

            // STAGE 1 - RESOLVE CLASS & NAMESPACE NAME
            // Here we extract the name of tested class and name of its namespace
            var namespaceName = NamespaceAnalyzer
                .ExtractFixtureNamespace(tree)
                .ToModuleNamespace();
            var className = FixtureNameAnalyzer
                .ExtractFixtureName(tree)
                .ToTargetClassName();

            // STAGE 2 - FIND MISSING MEMBERS
            // Here we create empty class in proper namespace, without any members.
            // To check, which members are needed, we investigate compilation messages.
            var emptyClassCompilation = CompileEmptyClass(tree, namespaceName, className);
            var missingMembers = emptyClassCompilation.GetMissingMembers();

            // STAGE 3 - SYNTAX-BASED - FIND SPECIFICATION OF MISSING MEMBERS
            (var properties, var functions) = MissingMembersAnalyzer.ResolveMissingMembers(missingMembers, tree, emptyClassCompilation);

            return new ClassDescription(className, namespaceName, properties, functions);
        }

        private static CSharpCompilation CompileEmptyClass(SyntaxTree tree, string namespaceName, string className)
        {
            var description = new ClassDescription(className, namespaceName, new PropertyDescription[] { }, new FunctionDescription[] { });
            var generator = new ClassGenerator(description);
            var emptyClassStub = generator.GenerateClass();
            var stubTree = CSharpSyntaxTree.ParseText(emptyClassStub);
            return new[] {tree, stubTree}.Compile();
        }
    }
}
