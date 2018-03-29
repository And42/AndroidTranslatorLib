using System.Xml;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    public sealed class XmlArrayString : OneXmlString, IXmlArrayString
    {
        public const string XmlNodeName = "array";

        /// <inheritdoc cref="OneString.IsChanged" />
        public override bool IsChanged => !string.IsNullOrEmpty(NewText);

        private readonly int _indexInArray;

        private readonly XmlNode _node;

        /// <inheritdoc />
        public XmlArrayString(XmlNode node, string name, int index): base(name, node.InnerText)
        {
            _node = node;
            _indexInArray = index;
        }

        /// <inheritdoc cref="OneXmlString.SaveChanges" />
        public override void SaveChanges()
        {
            if (!IsChanged)
                return;

            _node.InnerText = NewText;

            ApplyChanges();
        }

        /// <inheritdoc cref="OneXmlString.EqualsNavigations" />
        public override bool EqualsNavigations(IOneXmlString value)
        {
            // ReSharper disable once UsePatternMatching
            var val = value as XmlArrayString;

            if (val == null)
                return false;

            return Name == val.Name && _indexInArray == val._indexInArray;
        }
    }
}
