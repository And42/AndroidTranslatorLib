using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Strings
{
    public abstract class OneXmlString : OneString, IOneXmlString
    {
        /// <inheritdoc cref="IOneString.IsNewTextReadOnly" />
        public override bool IsNewTextReadOnly { get; } = false;

        /// <inheritdoc cref="IOneString.IsOldTextReadOnly" />
        public override bool IsOldTextReadOnly { get; } = true;

        /// <inheritdoc />
        public abstract void SaveChanges();

        /// <inheritdoc />
        public abstract bool EqualsNavigations(IOneXmlString value);

        /// <inheritdoc />
        protected OneXmlString(string name, string oldText): base(name, oldText){ }
    }
}
