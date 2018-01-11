using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Namespace;

namespace Sample.Test.Namespace
{
    [TestClass]
    public class SampleTest
    {
        private Sample _sample;

        [TestInitialize]
        public void SetUp()
        {
            _sample = new Sample();
        }

        [TestMethod]
        public void throws_not_implemented()
        {
            Assert.ThrowsException<NotImplementedException>(
                () => _sample.DoSmth("blah"));
        }

        [TestMethod]
        public void does_smth_else()
        {
            _sample.Blah = "blah";
            int result = _sample.DoSmthElse(_sample.Wololo);
        }

        [TestMethod]
        public void does_smth_else_without_explicit_type()
        {
            var result = _sample.DoSmthElse(_sample.Wololo);
            Assert.AreEqual(3, result);
        }
    }
}
