using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Interfaces.Files
{
    public interface IDictionaryFile : IEditableFile<IOneDictionaryString>
    {
        /// <summary>
        /// Добавляет строки из словаря
        /// </summary>
        /// <param name="file">Файл</param>
        void AddStringsFromFile(IDictionaryFile file);

        /// <summary>
        /// Добавляет изменённые строки из файла
        /// </summary>
        /// <param name="file"></param>
        void AddChangedStringsFromFile(IEditableFile file);

        /// <summary>
        /// Удаляет элемент по указанному индексу коллекции.
        /// </summary>
        /// <param name="index">Начинающийся с нуля индекс элемента, который требуется удалить.</param>
        void RemoveAt(int index);

        /// <summary>
        /// Удаляет строку из списка
        /// </summary>
        /// <param name="str">Строка</param>
        void Remove(IOneDictionaryString str);

        /// <summary>
        /// Удаляет все элементы из коллекции.
        /// </summary>
        void ClearItems();

        /// <summary>
        /// Добавляет новую строку в коллекцию
        /// </summary>
        /// <param name="oldText">Старый текст</param>
        /// <param name="newText">Новый текст</param>
        void Add(string oldText, string newText);
    }
}