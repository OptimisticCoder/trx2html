// <copyright file="ResourceReaderTestTest.cs" company="">Copyright ©rido  2007</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using trx2html.Test;

namespace trx2html.Test
{
    /// <summary>This class contains parameterized unit tests for ResourceReaderTest</summary>
    [PexClass(typeof(ResourceReaderTest))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ResourceReaderTestTest
    {
        /// <summary>Test stub for LoadTextFromResourceTest()</summary>
        [PexMethod]
        public void LoadTextFromResourceTest([PexAssumeUnderTest]ResourceReaderTest target)
        {
            target.LoadTextFromResourceTest();
            // TODO: add assertions to method ResourceReaderTestTest.LoadTextFromResourceTest(ResourceReaderTest)
        }

        /// <summary>Test stub for StreamFromResourceTest()</summary>
        [PexMethod]
        public void StreamFromResourceTest([PexAssumeUnderTest]ResourceReaderTest target)
        {
            target.StreamFromResourceTest();
            // TODO: add assertions to method ResourceReaderTestTest.StreamFromResourceTest(ResourceReaderTest)
        }

        /// <summary>Test stub for TestContext</summary>
        [PexMethod]
        public void TestContextGetSet(
            [PexAssumeUnderTest]ResourceReaderTest target,
            TestContext value
        )
        {
            target.TestContext = value;
            TestContext result = target.TestContext;
            PexAssert.AreEqual<TestContext>(value, result);
            // TODO: add assertions to method ResourceReaderTestTest.TestContextGetSet(ResourceReaderTest, TestContext)
        }
    }
}
