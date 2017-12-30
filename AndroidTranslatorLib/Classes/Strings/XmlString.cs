using System.Xml;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    public sealed class XmlString : OneXmlString, IXmlString
    {
        public const string XmlNodeName = "string";

        /// <inheritdoc />
        public override bool IsChanged => !string.IsNullOrEmpty(NewText);

        private readonly XmlNode _node;

        public XmlString(XmlNode node) : base(node.Attributes["name"].Value, node.InnerText)
        {
            _node = node;
        }

        public override void SaveChanges()
        {
            if (!IsChanged)
                return;

            _node.InnerText = NewText;

            ApplyChanges();
        }

        public override bool EqualsNavigations(IOneXmlString value)
        {
            var val = value as XmlString;

            if (val == null)
                return false;

            return val.Name == Name;
        }
    }
}
