using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using AndroidTranslator.Interfaces.Collections;
using AndroidTranslator.Interfaces.Strings;

// ReSharper disable once CheckNamespace
namespace AndroidTranslator
{
    /// <summary>
    /// Колекция строк словаря
    /// </summary>
    public sealed class DictionaryCollection : ReadOnlyObservableCollection<IOneString>, IDictionaryCollection
    {
        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="DictionaryCollection"/> на основе коллекции строк
        /// </summary>
        /// <param name="list">Коллекция строк</param>
        public DictionaryCollection(ObservableCollection<IOneString> list) : base(list) { }

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="DictionaryCollection"/>
        /// </summary>
        public DictionaryCollection() : base(new ObservableCollection<IOneString>()) { }

        /// <summary>
        /// Добавляет элементы в словарь
        /// </summary>
        /// <param name="items">Коллекция элементов</param>
        public void AddRange(IList<IOneDictionaryString> items)
        {
            foreach (var item in items)
                Items.Add(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        /// <summary>
        /// Добавляет один элемент в словарь
        /// </summary>
        /// <param name="str">Элемент</param>
        public void Add(IOneDictionaryString str)
        {
            Items.Add(str);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new[] {str}));
        }

        /// <summary>
        /// Удаляет один элемент из словаря
        /// </summary>
        /// <param name="str">Элемент</param>
        public void Remove(IOneDictionaryString str)
        {
            Items.Remove(str);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] {str}));
        }

        /// <summary>
        /// Удаляет элемент из словаря по указанному индексу
        /// </summary>
        /// <param name="index">Позиция элемента в коллекции</param>
        public void RemoveAt(int index)
        {
            var str = Items[index];

            Items.RemoveAt(index);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new[] {str}));
        }

        /// <summary>
        /// Удаляет все элементы из словаря
        /// </summary>
        public void Clear()
        {
            Items.Clear();

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
