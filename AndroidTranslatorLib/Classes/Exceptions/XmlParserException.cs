using System;

namespace AndroidTranslator.Classes.Exceptions
{
    [Serializable]
    public class XmlParserException : Exception
    {
        public XmlParserException(string message) : base(message) { }
    }
}
