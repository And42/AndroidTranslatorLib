using System;
using System.Collections.Generic;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator
{
    public static class ExtensionFunctions
    {
        /// <summary>
        /// Выполняет бинарный поиск по элементам <see cref="IOneString.OldText"/> списка типа <see cref="IOneString"/>
        /// Wiki: https://ru.wikipedia.org/wiki/%D0%94%D0%B2%D0%BE%D0%B8%D1%87%D0%BD%D1%8B%D0%B9_%D0%BF%D0%BE%D0%B8%D1%81%D0%BA
        /// </summary>
        /// <param name="array">Список</param>
        /// <param name="startIndex">Индекс начала поиска (включительно)</param>
        /// <param name="endIndex">Индекс конца поиска (не включительно)</param>
        /// <param name="searchValue">Значение для поиска</param>
        /// <returns>Индекс найденного элемента или -1, если элемент не содержится в списке</returns>
        public static int BinSearchInDetails(this IList<IOneString> array, string searchValue, int startIndex = 0, int endIndex = -1)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Count == 0)
                return -1;

            if (startIndex < 0 || startIndex > array.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (endIndex == -1)
                endIndex = array.Count;

            if (endIndex <= 0 || endIndex > array.Count || endIndex <= startIndex)
                throw new ArgumentOutOfRangeException(nameof(endIndex));

            foreach (var element in array)
                if (element.OldText == null)
                    return -1;

            if (string.Compare(searchValue, array[0].OldText, StringComparison.Ordinal) == -1 ||
                string.Compare(searchValue, array[array.Count - 1].OldText, StringComparison.Ordinal) == 1)
                return -1;

            // Приступить к поиску.
            // Номер первого элемента в массиве.
            int first = startIndex;
            // Номер элемента массива, СЛЕДУЮЩЕГО за последним
            int last = endIndex;

            // Если просматриваемый участок не пуст, first < last
            while (first < last)
            {
                int mid = first + ((last - first) >> 1);

                if (string.Compare(searchValue, array[mid].OldText, StringComparison.Ordinal) < 1)
                    last = mid;
                else
                    first = mid + 1;
            }

            // Теперь last может указывать на искомый элемент массива.
            if (array[last].OldText.Equals(searchValue))
                return last;

            return -1;
        }

        internal static bool Contains(this IEnumerable<string> list, string searchItem)
        {
            foreach (var str in list)
                if (str == searchItem)
                    return true;

            return false;
        }
    }
}
