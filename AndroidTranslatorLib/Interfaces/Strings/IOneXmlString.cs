namespace AndroidTranslator.Interfaces.Strings
{
    public interface IOneXmlString : IOneString
    {
        /// <summary>
        /// Сохраняет изменения в файл
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Сравнивает списки навигации двух элементов
        /// </summary>
        /// <param name="value">Обьект для сравнения</param>
        /// <returns></returns>
        bool EqualsNavigations(IOneXmlString value);
    }
}