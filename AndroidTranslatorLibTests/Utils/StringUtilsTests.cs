using System.Diagnostics;
using AndroidTranslator.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AndroidTranslatorLibTest.Utils
{
    [TestClass]
    public class StringUtilsTests
    {
        [TestMethod]
        public void FormatLineEndingsTest()
        {
            (string source, string expected)[] tests = 
            {
                ("", ""),
                ("\r", "\n"),
                ("\n", "\n"),
                ("\r\n", "\n"),
                ("\r \n", "\n \n"),
                ("test\ryea", "test\nyea"),
                ("test\nyea", "test\nyea"),
                ("test\r\nyea", "test\nyea"),
                ("test\r \nyea", "test\n \nyea"),
                ("\ryea", "\nyea"),
                ("\nyea", "\nyea"),
                ("\r\nyea", "\nyea"),
                ("\r \nyea", "\n \nyea"),
                ("test\r", "test\n"),
                ("test\n", "test\n"),
                ("test\r\n", "test\n"),
                ("test\r \n", "test\n \n")
            };

            foreach (var (source, expected) in tests)
            {
                Trace.WriteLine($"Current source: `{source}`, expected `{expected}`");
                Assert.AreEqual(expected, StringUtils.FormatLineEndings(source));
            }
        }
    }
}
