using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using AndroidTranslator.Classes.Exceptions;
using AndroidTranslator.Classes.Strings;
using AndroidTranslator.Interfaces.Files;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Files
{
    /// <summary>
    /// Класс для работы со строками в xml файлах
    /// </summary>
    public class XmlFile : EditableFile<IOneXmlString>, IXmlFile
    {
        /// <summary>
        /// Правила xml
        /// </summary>
        public static List<string> XmlRules { get; set; } =
            new List<string>
            {
                "android:text", "android:title", "android:summary", "android:dialogTitle",
                "android:summaryOff", "android:summaryOn", "value"
            };

        private readonly List<string> _xmlRules;
        private XmlDocument _xDoc;

        /// <summary>
        /// Загружать ли текстовые строки
        /// </summary>
        public bool LoadTextStrings { get; set; }

        /// <summary>
        /// Возвращает статус коллекции
        /// </summary>
        public override bool IsChanged => Details?.Any(str => str.IsChanged) == true;

        /// <summary>
        /// Список строк в файле
        /// </summary>
        public override ReadOnlyObservableCollection<IOneString> Details => _details;
        private ReadOnlyObservableCollection<IOneString> _details;

        /// <summary>
        /// Создаёт экземпляр класса XmlFile на основе файла и статического члена правил xml
        /// </summary>
        /// <param name="fileName">Полный путь к файлу</param>
        public static XmlFile Create(string fileName)
        {
            return new XmlFile(fileName, XmlRules);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса XmlFile
        /// </summary>
        /// <param name="fileName">Полный путь к файлу</param>
        /// <param name="xmlRules">Правила для текстовых строк</param>
        /// <param name="loadTextStrings">Загружать ли текстовые строки</param>
        public XmlFile(string fileName, List<string> xmlRules = null, bool loadTextStrings = false)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

            _xmlRules = xmlRules ?? new List<string>();
            FileName = fileName;
            LoadTextStrings = loadTextStrings;

            LoadStrings();
        }

        /// <summary>
        /// Загружает строки из файла на диске
        /// </summary>
        private void LoadStrings()
        {
            var list = new List<IOneXmlString>();

            if (FileName == null)
                return;

            _xDoc = new XmlDocument();

            try
            {
                _xDoc.Load(FileName);
            }
            catch (Exception ex)
            {
                throw new XmlParserException(ex.ToString());
            }

            if (_xDoc.DocumentElement?.Name == "resources")
            {
                foreach (XmlNode node in _xDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case XmlString.XmlNodeName:
                            list.Add(new XmlString(node));
                            break;
                        case XmlArrayString.XmlNodeName:
                            list.AddRange(node.ChildNodes.Cast<XmlNode>().Select((n, index) => new XmlArrayString(n, node.Attributes?["name"].Value, index)));
                            break;
                        case XmlStringsArrayString.XmlNodeName:
                            list.AddRange(node.ChildNodes.Cast<XmlNode>().Select((n, index) => new XmlStringsArrayString(n, node.Attributes?["name"].Value, index)));
                            break;
                        case XmlPluralsString.XmlNodeName:
                            list.AddRange(node.ChildNodes.Cast<XmlNode>().Select((n, index) => new XmlPluralsString(n, node.Attributes?["name"].Value, index)));
                            break;
                    }
                }
                    //list.Add(new XmlTextString(node, new List<int>(1) { i++ }));
            }
            /*else
            if (new[] { "arrays.xml", "plurals.xml" }.Any(s => s == Path.GetFileName(FileName)))
            {
                var items = GetTextStrings(xDoc.DocumentElement);
                foreach (var item in items)
                    list.Add(item);
            }
            else*/
            {
                var items = GetAttributeStrings(_xDoc.DocumentElement, _xmlRules);
                list.AddRange(items);

                if (LoadTextStrings)
                {
                    var collect = GetTextStrings(_xDoc.DocumentElement);
                    list.AddRange(collect);
                }
            }

            _details = 
                new ReadOnlyObservableCollection<IOneString>(
                    new ObservableCollection<IOneString>(
                        list.Where(it => !it.OldText.StartsWith("@"))
                    )
                );
        }

        /// <summary>
        /// Сохраняет изменения в файле на диск
        /// </summary>
        public override void SaveChanges()
        {
            foreach (var str in SpecDetails)
                str.SaveChanges();

            _xDoc.Save(FileName);
        }

        private IEnumerable<XmlTextString> GetTextStrings(XmlNode node)
        {
            var result = new List<XmlTextString>();
            _getTextStrings(node, ref result, new List<int>(), -1);
            return result;
        }

        private void _getTextStrings(XmlNode node, ref List<XmlTextString> result, IEnumerable<int> navigationList, int pos)
        {
            var navList = new List<int>(navigationList);
            if (pos > -1) navList.Add(pos);

            int i = 0;
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.HasChildNodes && (child.FirstChild as XmlText) != null && !string.IsNullOrEmpty(child.InnerText) && child.InnerText[0] != '@')
                    result.Add(new XmlTextString(child, new List<int>(navList) { i }));
                _getTextStrings(child, ref result, navList, i);
                i++;
            }
        }

        private static IEnumerable<XmlAttributeString> GetAttributeStrings(XmlNode node, IList<string> attributeNames)
        {
            var result = new List<XmlAttributeString>();

            _getAttributeStrings(node, attributeNames, ref result, new List<int>(), -1);

            return result;
        }

        private static void _getAttributeStrings(XmlNode node, IList<string> attributeNames, ref List<XmlAttributeString> result, IEnumerable<int> navigationList, int pos)
        {
            var navList = new List<int>(navigationList);

            if (pos > -1)
                navList.Add(pos);
            
            int i = 0;
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Attributes != null)
                {
                    int i2 = 0;
                    foreach (XmlAttribute attrib in child.Attributes)
                    {
                        // Ускорение в полтора раза
                        // ReSharper disable once LoopCanBeConvertedToQuery
                        foreach (var name in attributeNames)
                            if (name == attrib.Name)
                            {
                                if (attrib.InnerText.Length > 0 && attrib.InnerText[0] != '@')
                                    result.Add(new XmlAttributeString(attrib, new List<int>(navList) { i }, i2));
                                break;
                            }
                        i2++;
                    }
                }

                _getAttributeStrings(child, attributeNames, ref result, navList, i);
                i++;
            }
        }
    }
}
