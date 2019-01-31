using System.IO;
using System.Text;
using AndroidTranslator.Classes.Exceptions;
using AndroidTranslator.Classes.Files;
using LongPaths.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AndroidTranslatorLibTest
{
    [TestClass]
    public class Tests
    {
        private static string CreateTempXml(string content)
        {
            var tempFile = Path.GetTempFileName();

            LFile.WriteAllText(tempFile, content, Encoding.UTF8);

            return tempFile;
        }

        private static string CreateTempSmali(byte[] content)
        {
            var tempFile = Path.GetTempFileName();

            LFile.WriteAllBytes(tempFile, content);

            return tempFile;
        }

        [TestMethod]
        public void AndroidTranslatorLib_Strings()
        {
            var tempFile = CreateTempXml(TestResources.test_1_strings);

            XmlFile xfile = XmlFile.Create(tempFile);

            Assert.AreEqual(318, xfile.Details.Count);

            LFile.Delete(tempFile);
        }

        [TestMethod]
        public void AndroidTranslatorLib_Arrays()
        {
            var tempFile = CreateTempXml(TestResources.test_2_arrays);

            var xfile = XmlFile.Create(tempFile);

            Assert.AreEqual(60, xfile.Details.Count);
            Assert.AreEqual("Google речь в текст", xfile.Details[0].OldText);
            Assert.AreEqual("Пользовательская речь в текст", xfile.Details[1].OldText);
            Assert.AreEqual("0", xfile.Details[2].OldText);
            Assert.AreEqual("1", xfile.Details[3].OldText);

            LFile.Delete(tempFile);
        }

        [TestMethod]
        public void AndroidTranslatorLib_DictionaryTest()
        {
            var tempFile = CreateTempXml(TestResources.test_3_strings);

            var xfile = XmlFile.Create(tempFile);

            Assert.AreEqual(1, xfile.Details.Count);
            Assert.AreEqual("hello", xfile.Details[0].OldText);

            xfile.Details[0].NewText = "tree";

            var tempDict = CreateTempXml(TestResources.test_3_dict);

            var dict = new DictionaryFile(tempDict);
            dict.AddChangedStringsFromFile(xfile);

            Assert.AreEqual("hello", dict.Details[0].OldText);
            Assert.AreEqual("tree", dict.Details[0].NewText);

            xfile.Details[0].NewText = "more trees";

            dict.AddChangedStringsFromFile(xfile);

            Assert.AreEqual(1, dict.Details.Count);
            Assert.AreEqual("hello", dict.Details[0].OldText);
            Assert.AreEqual("more trees", dict.Details[0].NewText);

            LFile.Delete(tempFile);
            LFile.Delete(tempDict);
        }

        [TestMethod]
        public void AndroidTranslatorLib_InvalidFileTest()
        {
            var tempFile = CreateTempXml(TestResources.test_4_log_xml);

            AssertUtils.Throws<XmlParserException>(() => XmlFile.Create(tempFile));

            LFile.Delete(tempFile);

            tempFile = CreateTempSmali(TestResources.test_4_log_smali);

            AssertUtils.NotThrows(() => new SmaliFile(tempFile));

            LFile.Delete(tempFile);
        }

        [TestMethod]
        public void AndroidTranslatorLib_JumboStringsTest()
        {
            var tempFile = CreateTempSmali(TestResources.test_5_AMapLocationClient);

            var smali = new SmaliFile(tempFile);

            Assert.AreEqual(42, smali.Details.Count);

            LFile.Delete(tempFile);
        }

        [TestMethod]
        public void AndroidTranslatorLib_UniqueStringsTest()
        {
            var tempFile = CreateTempXml(TestResources.test_6_strings);

            var xml = XmlFile.Create(tempFile);

            Assert.AreEqual(147, xml.Details.Count);

            LFile.Delete(tempFile);
        }
    }
}
