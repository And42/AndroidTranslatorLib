using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AndroidTranslator.Interfaces.Files;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Files
{
    /// <summary>
    /// Класс для работы со строками в словарях
    /// </summary>
    public class DictionaryFile : EditableFile<IOneDictionaryString>, IDictionaryFile
    {
        /// <summary>
        /// Список строк в файле
        /// </summary>
        public override ReadOnlyObservableCollection<IOneString> Details => _dictDetails;
        private readonly DictionaryCollection _dictDetails = new DictionaryCollection();

        /// <summary>
        /// Возвращает, была ли изменена коллекция
        /// </summary>
        public override bool IsChanged => _isChanged || Details.Any(str => str.IsChanged);

        private const string InitXmlText =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\n<translations name=\"AeDict\" version=\"1.0\"/>";

        private XDocument _xdoc;
        private bool _isChanged;

        /// <summary>
        /// Создаёт экземпляр класса DictionaryFile на основе файла
        /// </summary>
        /// <param name="fileName">Файл</param>
        public DictionaryFile(string fileName)
        {
            FileName = fileName;

            var fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists && fileInfo.Length != 0)
                LoadStrings();
            else 
                _xdoc = XDocument.Parse(InitXmlText);
        }

        /// <summary>
        /// Создаёт новый экземпляр класса DictionaryFile
        /// </summary>
        public DictionaryFile()
        {
            _xdoc = XDocument.Parse(InitXmlText);
        }

        /// <summary>
        /// Добавляет строки из словаря
        /// </summary>
        /// <param name="file">Файл</param>
        public void AddStringsFromFile(IDictionaryFile file)
        {
            foreach (var str in file.Details)
                Add(str.OldText, str.NewText);
        }

        /// <summary>
        /// Добавляет изменённые строки из файла
        /// </summary>
        /// <param name="file"></param>
        public void AddChangedStringsFromFile(IEditableFile file)
        {
            foreach (var str in file.Details)
                if (str.IsChanged)
                    Add(str.OldText, str.NewText);
        }

        /// <summary>
        /// Сохраняет изменения в файле на диск
        /// </summary>
        public override void SaveChanges()
        {
            if (FileName == null)
                throw new FileNotFoundException("Dictionary file");

            _xdoc.Save(FileName);

            foreach (var str in Details)
                str.ApplyChanges();
        }

        /// <summary>
        /// Загружает строки из файла на диске
        /// </summary>
        private void LoadStrings()
        {
            _xdoc = XDocument.Load(FileName);
            if (_xdoc.Root == null) return;

            foreach (XElement el in _xdoc.Root.Elements())
                _dictDetails.Add(new OneDictionaryString(_xdoc, xElem: el));
        }

        /// <summary>
        /// Удаляет элемент по указанному индексу коллекции.
        /// </summary>
        /// <param name="index">Начинающийся с нуля индекс элемента, который требуется удалить.</param>
        public void RemoveAt(int index)
        {
            ((IOneDictionaryString)_dictDetails[index]).Delete();
            _dictDetails.RemoveAt(index);

            _isChanged = true;
        }

        /// <summary>
        /// Удаляет строку из списка
        /// </summary>
        /// <param name="str">Строка</param>
        public void Remove(IOneDictionaryString str)
        {
            if (_dictDetails.Contains(str))
            {
                str.Delete();
                _dictDetails.Remove(str);

                _isChanged = true;
            }
        }

        /// <summary>
        /// Удаляет все элементы из коллекции.
        /// </summary>
        public void ClearItems()
        {
            _xdoc.Root?.RemoveNodes();
            _dictDetails.Clear();

            _isChanged = true;
        }

        /// <summary>
        /// Добавляет новую строку в коллекцию
        /// </summary>
        /// <param name="oldText">Старый текст</param>
        /// <param name="newText">Новый текст</param>
        public void Add(string oldText, string newText)
        {
            var f = Details.FirstOrDefault(str => str.OldText == oldText);

            if (f == null)
                _dictDetails.Add(new OneDictionaryString(_xdoc, oldText, newText));
            else
                f.NewText = newText;

            _isChanged = true;
        }
    }
}
