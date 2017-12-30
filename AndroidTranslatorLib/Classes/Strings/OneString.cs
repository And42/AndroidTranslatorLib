using System;
using System.Collections.Generic;
using System.ComponentModel;
using AndroidTranslator.Annotations;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    /// <summary>
    /// Базовый класс для одной строки
    /// </summary>
    public abstract class OneString : IOneString
    {
        /// <inheritdoc />
        public virtual bool IsOldTextReadOnly { get; } = true;

        /// <inheritdoc />
        public virtual bool IsNewTextReadOnly { get; } = false;

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возвращает или задаёт старый текст
        /// </summary>
        public virtual string OldText
        {
            get => _oldText;
            set
            {
                if (IsOldTextReadOnly)
                    throw new InvalidOperationException(nameof(NewText) + " is read only!");

                SetOldText(value); 
            }
        }
        private string _oldText;

        /// <summary>
        /// Возвращает или задаёт новый текст
        /// </summary>
        public virtual string NewText
        {
            get => _newText;
            set
            {
                if (IsNewTextReadOnly)
                    throw new InvalidOperationException(nameof(NewText) + " is read only!");

                SetNewText(value);
            }
        }
        private string _newText;

        /// <inheritdoc />
        public abstract bool IsChanged { get; }

        /// <summary>
        /// Создаёт экземпляр класса OneString
        /// </summary>
        protected OneString() { }

        /// <summary>
        /// Создаёт экземпляр класса OneString на основе названия и старого текста
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="oldText">Старый текст</param>
        protected OneString(string name, string oldText)
        {
            Name = name;
            _oldText = oldText;
        }

        protected void SetOldText(string text)
        {
            SetProperty(ref _oldText, text, nameof(OldText));
        }

        protected void SetNewText(string text)
        {
            SetProperty(ref _newText, text, nameof(NewText));
        }

        /// <summary>
        /// Применяет изменения
        /// </summary>
        public virtual void ApplyChanges()
        {
            SetOldText(NewText);
            SetNewText(string.Empty);
        }

#pragma warning disable 1591

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

#pragma warning restore 1591
    }
}
