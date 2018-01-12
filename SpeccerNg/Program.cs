using System;
using System.IO;
using Speccer.Analysis;
using Speccer.Generation;

namespace SpeccerNg
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                Console.WriteLine($"Usage: {Environment.CommandLine.Split(' ')[0]} path_to_test_file");
                return;
            }
            var fixtureContent = File.ReadAllText(args[0]);
            var description = FixtureAnalyzer.ExtractSpecification(fixtureContent);
            var generator = new ClassGenerator(description);
            var code = generator.GenerateClass();
            var classPath = description.Name + ".cs";
            if (File.Exists(classPath))
            {
                Console.WriteLine($"{classPath} already exists. Overwrite? [Y]es / [N]o ");
                var key = Console.ReadKey();
                if (key.KeyChar != 'Y' && key.KeyChar != 'y')
                    return;
            }
            File.WriteAllText(classPath, code);
        }
    }
}
