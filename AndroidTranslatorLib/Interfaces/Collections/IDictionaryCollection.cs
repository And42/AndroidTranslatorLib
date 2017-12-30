using System.Collections.Generic;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Interfaces.Collections
{
    public interface IDictionaryCollection
    {
        /// <summary>
        /// Добавляет элементы в словарь
        /// </summary>
        /// <param name="items">Коллекция элементов</param>
        void AddRange(IList<IOneDictionaryString> items);

        /// <summary>
        /// Добавляет один элемент в словарь
        /// </summary>
        /// <param name="str">Элемент</param>
        void Add(IOneDictionaryString str);

        /// <summary>
        /// Удаляет один элемент из словаря
        /// </summary>
        /// <param name="str">Элемент</param>
        void Remove(IOneDictionaryString str);

        /// <summary>
        /// Удаляет элемент из словаря по указанному индексу
        /// </summary>
        /// <param name="index">Позиция элемента в коллекции</param>
        void RemoveAt(int index);

        /// <summary>
        /// Удаляет все элементы из словаря
        /// </summary>
        void Clear();
    }
}