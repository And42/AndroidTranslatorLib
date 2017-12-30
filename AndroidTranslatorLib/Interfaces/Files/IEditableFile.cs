using System.Collections.ObjectModel;
using System.ComponentModel;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Interfaces.Files
{
    public interface IEditableFile : INotifyPropertyChanged
    {
        /// <summary>
        /// Полный путь до файла на диске
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Список строк в файле
        /// </summary>
        ReadOnlyObservableCollection<IOneString> Details { get; }

        /// <summary>
        /// Определяет, изменёны ли строки в файле
        /// </summary>
        bool IsChanged { get; }

        /// <summary>
        /// Сохраняет изменения в файле на диск
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Переводит строки с помощью словаря
        /// </summary>
        /// <param name="dictionary">Словарь для перевода</param>
        /// <param name="saveChanges">Сохранять ли изменения после перевода</param>
        void TranslateWithDictionary(IDictionaryFile dictionary, bool saveChanges = false);
    }
}