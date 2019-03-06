using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.WebUi.TestData;
using System.Xml.Linq;

namespace Benday.Presidents.UnitTests
{
    // [TestClass]
    public class TestDataResourceFixture
    {
        // [TestMethod]
        public void ReadPresidentsXmlFromResource()
        {
            Console.WriteLine(TestDataResource.Culture);

            var actual = TestDataResource.us_presidents;

            Assert.IsNotNull(actual, "Us president xml data should not be null.");

            var doc = XDocument.Parse(actual);
        }

        // [TestMethod]
        public void ReadPresidentsXmlFromResourceShouldNotFailIfCultureIsNull()
        {
            TestDataResource.Culture = null;

            var actual = TestDataResource.ResourceManager.GetString("us_presidents");

            // var actual = TestDataResource.us_presidents;

            Assert.IsNotNull(actual, "Us president xml data should not be null.");

            var doc = XDocument.Parse(actual);
        }
    }
}
