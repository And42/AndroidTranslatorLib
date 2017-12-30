using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AndroidTranslatorLibTest
{
    internal static class AssertUtils
    {
        public static void NotThrows(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                Assert.Fail("Throws exception");
            }
        }

        public static void Throws<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception)
            {
                Assert.Fail("Throws<" + typeof(TException).Name + ">");
            }

            Assert.Fail("Throws<" + typeof(TException).Name + ">");
        }
    }
}
