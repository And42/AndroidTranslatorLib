using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Interfaces.Files
{
    public interface IXmlFile : IEditableFile<IOneXmlString>
    {
        /// <summary>
        /// Загружать ли текстовые строки
        /// </summary>
        bool LoadTextStrings { get; set; }
    }
}