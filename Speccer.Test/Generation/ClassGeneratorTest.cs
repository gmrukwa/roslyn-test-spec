using Microsoft.VisualStudio.TestTools.UnitTesting;
using Speccer.Description;
using Speccer.Generation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Speccer.Test.Generation
{
    [TestClass]
    public class ClassGeneratorTest
    {
        private ClassGenerator _classGenerator;

        [TestInitialize]
        public void SetUp()
        {
            string namespaceStr = "Namespace.SubNamespace";
            string nameStr = "ClassName";
            List<PropertyDescription> properties = new List<PropertyDescription>
            {
                new PropertyDescription("Property1", typeof(long), false),
                new PropertyDescription("Property2", typeof(string), true),
                new PropertyDescription("Property3", typeof(bool), true)
            };

            List<FunctionDescription> functions = new List<FunctionDescription>
            {
                new FunctionDescription("Method1", typeof(Object), new List<Type>()),
                new FunctionDescription("Method2", typeof(void), new List<Type>
                {
                    typeof(string),
                    typeof(int),
                    typeof(bool)
                }),
            };

            ClassDescription cd = new ClassDescription(
                nameStr,
                namespaceStr,
                properties,
                functions);

            _classGenerator = new ClassGenerator(cd);
        }

        [TestMethod]
        public void works()
        {
            string result = _classGenerator.GenerateClass();
            Console.WriteLine(result);
            Assert.IsNotNull(result);
        }
    }
}
