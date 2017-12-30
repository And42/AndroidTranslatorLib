using System;
using System.Collections.Generic;
using System.Linq;
using AndroidTranslator;
using AndroidTranslator.Interfaces.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AndroidTranslatorLibTest
{
    [TestClass]
    public class FunctionsUnitTests
    {
        [TestMethod]
        public void BinSearchInDetails_Test()
        {
            Mock<IOneString> MockString(string oldText)
            {
                var mockObj = new Mock<IOneString>();

                mockObj.SetupGet(str => str.OldText).Returns(oldText);

                return mockObj;
            }

            List<IOneString> MockStrings(params string[] oldTexts)
            {
                return oldTexts.Select(oldText => MockString(oldText).Object).ToList();
            }

            AssertUtils.Throws<ArgumentNullException>(() => ExtensionFunctions.BinSearchInDetails(null, null));

            AssertUtils.Throws<ArgumentNullException>(() => ExtensionFunctions.BinSearchInDetails(null, "1"));

            Assert.AreEqual(-1, MockStrings().BinSearchInDetails("1"));

            Assert.AreEqual(-1, MockStrings("2").BinSearchInDetails("1"));

            Assert.AreEqual(-1, MockStrings(null, "1", null).BinSearchInDetails("1"));

            Assert.AreEqual(0, MockStrings("1").BinSearchInDetails("1"));
            
            Assert.AreEqual(2, MockStrings("1", "2", "3").BinSearchInDetails("3"));
        }
    }
}