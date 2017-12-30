using System.ComponentModel;

namespace AndroidTranslator.Interfaces.Strings
{
    public interface IOneString : INotifyPropertyChanged
    {
        /// <summary>
        /// Возвращает возможность редактирования старого текста
        /// </summary>
        bool IsOldTextReadOnly { get; }

        /// <summary>
        /// Возвращает возможность редактирования нового текста
        /// </summary>
        bool IsNewTextReadOnly { get; }

        /// <summary>
        /// Название
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Возвращает или задаёт старый текст
        /// </summary>
        string OldText { get; set; }

        /// <summary>
        /// Возвращает или задаёт новый текст
        /// </summary>
        string NewText { get; set; }

        /// <summary>
        /// Возвращает, была ли изменена строка
        /// </summary>
        bool IsChanged { get; }

        /// <summary>
        /// Применяет изменения
        /// </summary>
        void ApplyChanges();
    }
}