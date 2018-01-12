﻿using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Speccer.Analysis;

namespace Speccer.Test.Analysis
{
    [TestClass]
    public class FixtureAnalyzerTest
    {
        private FixtureAnalyzer _analyzer;
        private static string _sampleTestFile;

        private static string FindTestFile(string startPath) => 
            File.Exists(Path.Combine(startPath, "SampleClassTest.cs"))
            ? Path.Combine(startPath, "SampleClassTest.cs")
            : FindTestFile(Path.Combine(startPath, ".."));

        [ClassInitialize]
        public static void SetUpOnce(TestContext context)
        {
            _sampleTestFile = File.ReadAllText(FindTestFile("."));
        }

        [TestInitialize]
        public void SetUp()
        {
            _analyzer = new FixtureAnalyzer();
        }

        [TestMethod]
        public void does_not_throw_on_sample_use()
        {
            _analyzer.ExtractSpecification(_sampleTestFile);
        }

        [TestMethod]
        public void parses_namespace()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual("Sample.Namespace", description.Namespace);
        }

        [TestMethod]
        public void parses_name()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual("SampleClass", description.Name);
        }

        [TestMethod]
        public void finds_functions()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(2, description.Functions.Count());
        }

        [TestMethod]
        public void finds_function_names()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.IsTrue(description.Functions.Any(f => f.Name == "DoSmth"));
            Assert.IsTrue(description.Functions.Any(f => f.Name == "DoSmthElse"));
        }

        [TestMethod]
        public void default_function_return_type_is_void()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(typeof(void), description.Functions.First(f => f.Name == "DoSmth").ReturnType);
        }

        [TestMethod]
        [Ignore]
        public void finds_return_types()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(typeof(int), description.Functions.First(f => f.Name == "DoSmthElse").ReturnType);
        }

        [TestMethod]
        [Ignore]
        public void finds_arguments_number()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(1, description.Functions.First(f => f.Name == "DoSmth").Arguments.Count());
        }

        [TestMethod]
        [Ignore]
        public void recognizes_argument_type()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(typeof(string), description.Functions.First(f => f.Name == "DoSmth").Arguments.First());
        }

        [TestMethod]
        [Ignore]
        public void default_argument_type_is_object()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(typeof(object), description.Functions.First(f => f.Name == "DoSmthElse").Arguments.First());
        }

        [TestMethod]
        public void finds_properties()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(2, description.Properties.Count());
        }

        [TestMethod]
        public void finds_property_name()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.IsTrue(description.Properties.Any(p => p.Name == "Blah"));
            Assert.IsTrue(description.Properties.Any(p => p.Name == "Wololo"));
        }

        [TestMethod]
        public void property_is_readonly_by_default()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.IsFalse(description.Properties.First(p => p.Name == "Wololo").HasSetter);
        }

        [TestMethod]
        public void recognizes_settable_properties()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.IsTrue(description.Properties.First(p => p.Name == "Blah").HasSetter);
        }

        [TestMethod]
        [Ignore]
        public void finds_property_type()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(typeof(string), description.Properties.First(p => p.Name == "Blah").Type);
        }

        [TestMethod]
        public void property_is_object_by_default()
        {
            var description = _analyzer.ExtractSpecification(_sampleTestFile);
            Assert.AreEqual(typeof(object), description.Properties.First(p => p.Name == "Wololo").Type);
        }
    }
}
