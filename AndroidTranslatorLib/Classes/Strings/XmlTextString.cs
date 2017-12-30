using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    /// <summary>
    /// Класс для работы с текстовой строкой xml файла
    /// </summary>
    public sealed class XmlTextString : OneXmlString, IXmlTextString
    {
        private readonly List<int> _childNavigation;
        private readonly XmlNode _node;

        /// <summary>
        /// Возвращает, была ли изменена строка
        /// </summary>
        public override bool IsChanged => !string.IsNullOrWhiteSpace(NewText);

        /// <summary>
        /// Создаёт экземпляр класса <see cref="XmlTextString"/> на основе элемента дерева и списка навигации
        /// </summary>
        /// <param name="node">Элемент</param>
        /// <param name="navigationList">Список навигации</param>
        public XmlTextString(XmlNode node, List<int> navigationList): base(node.Attributes?["name"]?.InnerText ?? node.Name, node.InnerText)
        {
            _node = node;
            _childNavigation = navigationList;
        }

        /// <summary>
        /// Сохраняет изменения в файл
        /// </summary>
        public override void SaveChanges()
        {
            if (!IsChanged)
                return;

            _node.InnerText = NewText;
            ApplyChanges();
        }

        /// <summary>
        /// Сравнивает списки навигации двух элементов
        /// </summary>
        /// <param name="value">Обьект для сравнения</param>
        public override bool EqualsNavigations(IOneXmlString value)
        {
            var item = value as XmlTextString;
            return item != null && _childNavigation.SequenceEqual(item._childNavigation);
        }
    }
}
