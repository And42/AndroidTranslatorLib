using System.Xml;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    public sealed class XmlPluralsString : OneXmlString, IXmlPluralsString
    {
        public const string XmlNodeName = "plurals";

        public override bool IsChanged => !string.IsNullOrEmpty(NewText);

        private readonly int _indexInArray;

        private readonly XmlNode _node;

        public XmlPluralsString(XmlNode node, string name, int index): base(name, node.InnerText)
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
            var val = value as XmlPluralsString;

            if (val == null)
                return false;

            return Name == val.Name && _indexInArray == val._indexInArray;
        }
    }
}
