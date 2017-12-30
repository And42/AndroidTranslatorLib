using System.Collections.Generic;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Interfaces.Files
{
	public interface IEditableFile<TStr> : IEditableFile where TStr : IOneString
    {
        /// <summary>
        /// Список строк в файле
        /// </summary>
        IEnumerable<TStr> SpecDetails { get; }
	}
}