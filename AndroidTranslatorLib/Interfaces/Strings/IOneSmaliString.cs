namespace AndroidTranslator.Interfaces.Strings
{
    public interface IOneSmaliString : IOneString
    {
        /// <summary>
        /// Позиция начала строки в файле
        /// </summary>
        int StartIndex { get; }

        /// <summary>
        /// Строка в файле
        /// </summary>
        int LineInFile { get; }

        /// <summary>
        /// Сохраняет изменения и возвращает изменённый файл
        /// </summary>
        /// <param name="fileLines">Строки файла</param>
        void SaveChanges(string[] fileLines);
    }
}