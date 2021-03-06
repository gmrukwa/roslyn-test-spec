﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Namespace;

namespace Sample.Test.Namespace
{
    [TestClass]
    public class SampleClassTest
    {
        private SampleClass _sample;

        [TestInitialize]
        public void SetUp()
        {
            _sample = new SampleClass();
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
