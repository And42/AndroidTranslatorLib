using System.Globalization;
using System.Text.RegularExpressions;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    /// <summary>
    /// Класс строки smali файла
    /// </summary>
    public sealed class OneSmaliString : OneString, IOneSmaliString
    {
        public override bool IsOldTextReadOnly { get; } = true;
        public override bool IsNewTextReadOnly { get; } = false;

        /// <summary>
        /// Позиция начала строки в файле
        /// </summary>
        public int StartIndex { get; }

        /// <summary>
        /// Строка в файле
        /// </summary>
        public int LineInFile { get; }

        private static readonly Regex UnicodeRegex = new Regex(@"\\[uU]([0-9A-Fa-f]{4})", RegexOptions.Compiled);

        private readonly string _uniSecText;
        private bool _isUniChanged;

        /// <summary>
        /// Создаёт экземпляр объекта OneSmaliString на основе названия, текста и позиции в тексте
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="oldText">Текст</param>
        /// <param name="startIndex">Позиция в тексте</param>
        /// <param name="lineInFile">Строка в файле</param>
        public OneSmaliString(string name, string oldText, int startIndex, int lineInFile): base(name, UnicodeStringToNet(oldText))
        {
            _uniSecText = oldText;
            _isUniChanged = oldText != OldText;
            StartIndex = startIndex;
            LineInFile = lineInFile;
        }

        /// <summary>
        /// Сохраняет изменения и возвращает изменённый файл
        /// </summary>
        /// <param name="fileLines">Строки файла</param>
        public void SaveChanges(string[] fileLines)
        {
            if (!IsChanged)
                return;

            string replText = _isUniChanged ? _uniSecText : OldText;

            fileLines[LineInFile] = fileLines[LineInFile].Remove(StartIndex, replText.Length);
            fileLines[LineInFile] = fileLines[LineInFile].Insert(StartIndex, NewText);

            _isUniChanged = false;

            ApplyChanges();
        }

        /*/// <summary>
        /// Сохраняет изменения и возвращает изменённый файл
        /// </summary>
        /// <param name="fileLines">Строки файла</param>
        /// <param name="list">Список всех строк</param>
        /// <param name="pos">Позиция строки в списке</param>
        public void SaveChanges(string[] fileLines, IList<OneString> list, int pos)
        {
            if (!IsChanged()) return textOfFile;

            string replText = isUniChanged ? uniSecText : OldText;

            textOfFile = textOfFile.Remove(StartIndex, replText.Length);
            textOfFile = textOfFile.Insert(StartIndex, NewText);
            MovePos(NewText.Length - replText.Length, list, pos, true);
            MoveNew();
            isUniChanged = false;
            return textOfFile;
        }*/

        /*/// <summary>
        /// Сдвигает начальные позиции в тексте всех строк
        /// </summary>
        /// <param name="plus">Сдвиг</param>
        /// <param name="list">Список всех строк</param>
        /// <param name="pos">Позиция текущей строки</param>
        /// <param name="start">Начальный ли элемент</param>
        public void MovePos(int plus, IList<OneString> list, int pos, bool start = false)
        {
            if (!start) StartIndex += plus;
            if (pos < list.Count - 1) list[pos + 1].As<OneSmaliString>().MovePos(plus, list, pos + 1);
        }*/

        /*
        /// <summary>
        /// Сохраняет изменения и возвращает изменённый файл
        /// </summary>
        /// <param name="textOfFile">Текст файла</param>
        /// <param name="list">Список всех строк</param>
        /// <param name="pos">Позиция строки в списке</param>
        public string SaveChanges(string textOfFile, IList<OneSmaliString> list, int pos)
        {
            if (!IsChanged()) return textOfFile;
            textOfFile = textOfFile.Remove(StartIndex, OldText.Length);
            textOfFile = textOfFile.Insert(StartIndex, NewText);
            MovePos(NewText.Length - OldText.Length, list, pos, true);
            MoveNew();
            return textOfFile;
        }*/

        /*/// <summary>
        /// Сдвигает начальные позиции в тексте всех строк
        /// </summary>
        /// <param name="plus">Сдвиг</param>
        /// <param name="list">Список всех строк</param>
        /// <param name="pos">Позиция текущей строки</param>
        /// <param name="start">Начальный ли элемент</param>
        public void MovePos(int plus, IList<OneSmaliString> list, int pos, bool start = false)
        {
            if (!start) StartIndex += plus;
            // ReSharper disable once PossibleNullReferenceException
            if (pos < list.Count - 1) list[pos + 1].MovePos(plus, list, pos + 1);
        }*/ 

        private static string UnicodeStringToNet(string input)
        {
            return UnicodeRegex.Replace(input, match => ((char)int.Parse(match.Groups[1].Value, NumberStyles.HexNumber)).ToString());
        }

        /// <summary>
        /// Возвращает, была ли изменена строка
        /// </summary>
        public override bool IsChanged => !string.IsNullOrWhiteSpace(NewText);
    }
}
