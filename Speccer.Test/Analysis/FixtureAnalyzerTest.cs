using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Speccer.Analysis;

namespace Speccer.Test.Analysis
{
    [TestClass]
    public class FixtureAnalyzerTest
    {
        private FixtureAnalyzer _analyzer;

        [TestInitialize]
        public void SetUp()
        {
            _analyzer = new FixtureAnalyzer();
        }

        [TestMethod]
        public void throws_not_implemented()
        {
            Assert.ThrowsException<NotImplementedException>(
                () => _analyzer.ExtractSpecification("blah"));
        }
    }
}
