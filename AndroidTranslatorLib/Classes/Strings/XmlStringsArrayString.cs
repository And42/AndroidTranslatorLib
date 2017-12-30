using System.Xml;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    public sealed class XmlStringsArrayString : OneXmlString, IXmlStringsArrayString
    {
        public const string XmlNodeName = "string-array";

        public override bool IsChanged => !string.IsNullOrEmpty(NewText);

        private readonly int _indexInArray;

        private readonly XmlNode _node;

        public XmlStringsArrayString(XmlNode node, string name, int index): base(name, node.InnerText)
        {
            _node = node;
            _indexInArray = index;
        }

        public override void SaveChanges()
        {
            if (!IsChanged) return;

            _node.InnerText = NewText;

            ApplyChanges();
        }

        public override bool EqualsNavigations(IOneXmlString value)
        {
            var val = value as XmlStringsArrayString;

            if (val == null)
                return false;

            return Name == val.Name && _indexInArray == val._indexInArray;
        }
    }
}
