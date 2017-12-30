using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    /// <summary>
    /// Класс для работы с атрибутной строкой xml файла
    /// </summary>
    public sealed class XmlAttributeString : OneXmlString, IXmlAttributeString
    {
        private readonly int _attributeNavigation;
        private readonly List<int> _childNavigation;
        private readonly XmlAttribute _attribute;

        /// <summary>
        /// Создаёт экземпляр класса <see cref="XmlAttributeString"/>
        /// </summary>
        /// <param name="attribute">Элемент</param>
        /// <param name="childNavigation">Список навигации</param>
        /// <param name="attributeNavigation">Номер атрибута в списке атрибутов</param>
        public XmlAttributeString(XmlAttribute attribute, List<int> childNavigation, int attributeNavigation): base(attribute.Name, attribute.InnerText)
        {
            _attribute = attribute;
            _childNavigation = childNavigation;
            _attributeNavigation = attributeNavigation;
        }

        /// <summary>
        /// Сохраняет изменения в файл
        /// </summary>
        public override void SaveChanges()
        {
            if (!IsChanged)
                return;

            _attribute.InnerText = NewText;
            ApplyChanges();
        }

        /// <summary>
        /// Сравнивает списки навигации двух элементов
        /// </summary>
        /// <param name="value">Обьект для сравнения</param>
        /// <returns></returns>
        public override bool EqualsNavigations(IOneXmlString value)
        {
            var item = value as XmlAttributeString;

            if (item == null)
                return false;

            return _childNavigation.SequenceEqual(item._childNavigation) && _attributeNavigation.Equals(item._attributeNavigation);
        }

        public override bool IsChanged => !string.IsNullOrWhiteSpace(NewText);
    }
}
