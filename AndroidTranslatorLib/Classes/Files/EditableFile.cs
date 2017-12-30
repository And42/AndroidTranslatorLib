using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AndroidTranslator.Annotations;
using AndroidTranslator.Interfaces.Files;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Files
{
    /// <summary>
    /// Базовый класс редактора строк в файлах
    /// </summary>
    public abstract class EditableFile<TStr>: IEditableFile<TStr> where TStr : class, IOneString
    {
        /// <summary>
        /// Полный путь до файла на диске
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// Список строк в файле
        /// </summary>
        public abstract ReadOnlyObservableCollection<IOneString> Details { get; }
 
        /// <summary>
        /// Список строк в файле
        /// </summary>
        public IEnumerable<TStr> SpecDetails => Details.Cast<TStr>();

        /// <summary>
        /// Определяет, изменёны ли строки в файле
        /// </summary>
        public virtual bool IsChanged => Details?.Any(str => str.IsChanged) == true;

        /// <summary>
        /// Сохраняет изменения в файле на диск
        /// </summary>
        public abstract void SaveChanges();

        /// <summary>
        /// Переводит строки с помощью словаря
        /// </summary>
        /// <param name="dictionary">Словарь для перевода</param>
        /// <param name="saveChanges">Сохранять ли изменения после перевода</param>
        public void TranslateWithDictionary(IDictionaryFile dictionary, bool saveChanges = false)
        {
            foreach (var str in Details)
            {
                var dictstr = dictionary.Details.FirstOrDefault(dstr => dstr.OldText == str.OldText);

                if (dictstr != null)
                    str.NewText = dictstr.NewText;
            }

            if (saveChanges)
                SaveChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
