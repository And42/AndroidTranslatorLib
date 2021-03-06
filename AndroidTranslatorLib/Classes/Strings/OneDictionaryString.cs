﻿using System;
using System.Xml.Linq;
using AndroidTranslator.Classes.Strings;
using AndroidTranslator.Interfaces.Strings;
using AndroidTranslator.Utils;

// ReSharper disable once CheckNamespace
namespace AndroidTranslator
{
    /// <summary>
    /// Класс для работы со строкой словаря
    /// </summary>
    public sealed class OneDictionaryString : OneString, IOneDictionaryString
    {
        /// <inheritdoc cref="IOneString.IsOldTextReadOnly" />
        public override bool IsOldTextReadOnly { get; } = false;

        /// <inheritdoc cref="IOneString.IsNewTextReadOnly" />
        public override bool IsNewTextReadOnly { get; } = false;

        private readonly XElement _node;

        private bool _isChanged;

        private string _oldText;

        /// <summary>
        /// Возвращает или задаёт старый текст
        /// </summary>
        public override string OldText
        {
            get => _oldText;
            set
            {
                var formatted = StringUtils.FormatLineEndings(value);

                if (SetProperty(ref _oldText, formatted, nameof(OldText)))
                {
                    _node.SetAttributeValue("from", formatted);
                    _isChanged = true;
                }
            }
        }

        private string _newText;

        /// <summary>
        /// Возвращает или задаёт новый текст
        /// </summary>
        public override string NewText
        {
            get => _newText;
            set
            {
                var formatted = StringUtils.FormatLineEndings(value);

                if (SetProperty(ref _newText, formatted, nameof(NewText)))
                {
                    _node.SetAttributeValue("to", formatted);
                    _isChanged = true;
                }
            }
        }

        /// <summary>
        /// Создаёт экземпляр класса OneDictionaryString
        /// </summary>
        /// <param name="xDoc">Документ</param>
        /// <param name="from">Старый текст</param>
        /// <param name="to">Новый текст</param>
        /// <param name="xElem">Элемент</param>
        public OneDictionaryString(XDocument xDoc, string from = "", string to = "", XElement xElem = null)
        {
            if (xDoc == null)
                throw new ArgumentNullException(nameof(xDoc));
            if (xDoc.Root == null)
                throw new ArgumentNullException(nameof(xDoc.Root));
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            if (xElem == null)
            {
                _node = 
                    new XElement("translate",
                        new XAttribute("from", from),
                        new XAttribute("to", to)
                    );

                xDoc.Root.Add(_node);

                _oldText = from;
                _newText = to;
            }
            else
            {
                _node = xElem;

                var fromAttrib = _node.Attribute("from");
                var toAttrib = _node.Attribute("to");

                if (fromAttrib == null)
                    _node.SetAttributeValue("from", _oldText = from);
                else
                    _oldText = StringUtils.FormatLineEndings(fromAttrib.Value);

                if (toAttrib == null)
                    _node.SetAttributeValue("to", _newText = to);
                else
                    _newText = StringUtils.FormatLineEndings(toAttrib.Value);
            }

            IsOldTextReadOnly = false;
            _isChanged = false;
        }

        /// <summary>
        /// Удаляет строку
        /// </summary>
        public void Delete()
        {
            _node.Remove();
        }

        /// <summary>
        /// Возвращает, была ли строка изменена
        /// </summary>
        public override bool IsChanged => _isChanged;

        /// <summary>
        /// Применяет изменения
        /// </summary>
        public override void ApplyChanges()
        {
            _isChanged = false;
        }
    }
}
