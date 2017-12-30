using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using AndroidTranslator.Classes.Strings;
using AndroidTranslator.Interfaces.Files;
using AndroidTranslator.Interfaces.Strings;

namespace AndroidTranslator.Classes.Files
{
    /// <summary>
    /// Класс для работы со строками в smali файлах
    /// </summary>
    public sealed class SmaliFile : EditableFile<IOneSmaliString>, ISmaliFile
    {
        private static readonly string[] SmaliCheckStrings = { "const-string v", "const-string/jumbo v", ":Ljava/lang/String; =" };
        private static readonly string[][] SmaliSides =
        {
            new[] { "const-string v", "," },
            new[] { "const-string/jumbo v", "," },
            new[] { ":Ljava/lang/String;", "=" }
        };
        private static readonly Encoding FileEncoding = new UTF8Encoding(false);
        private static readonly string[] ShortStrings = { "[", "]", ",", ":", "{", "}", "=", "<", ">" };

        private const int SpaceCountAfterComma = 1;

        private string[] _fileLines;

        /// <summary>
        /// Инициализирует новый экземпляр класса XmlFile
        /// </summary>
        /// <param name="fileName">Полный путь к файлу</param>
        ///// <param name="replaceSequences">Показывает, нужно ли заменить Unicode последовательности на символы</param>
        public SmaliFile(string fileName/*, bool replaceSequences = true*/)
        {
            FileName = fileName;
            LoadStrings();
            /*if (replaceSequences)
                ReplaceSeqInDetails();*/
        }

        /// <summary>
        /// Возвращает, содержатся ли в файле строки
        /// </summary>
        public static bool HasLines(string file)
        {
            using (var reader = new StreamReader(file, FileEncoding))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    foreach (var checkItem in SmaliCheckStrings)
                        if (line.Contains(checkItem))
                            return true;
                }
            }

            return false;
        }

        /*/// <summary>
        /// Заменяет последовательности Unicode символов в строках
        /// </summary>
        public void ReplaceSeqInDetails()
        {
            Details?.As<SmaliCollection>().ReplaceSequences();
        }*/

        /// <inheritdoc />
        public override ReadOnlyObservableCollection<IOneString> Details => _details;
        private ReadOnlyObservableCollection<IOneString> _details;

        /// <summary>
        /// Сохраняет изменения в файле на диск
        /// </summary>
        public override void SaveChanges()
        {
            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (var item in SpecDetails)
                item.SaveChanges(_fileLines);

            File.WriteAllLines(FileName, _fileLines, new UTF8Encoding(false));
        }
        //private string textOfFile;

        /// <summary>
        /// Загружает строки из файла на диске
        /// </summary>
        private void LoadStrings(bool ignoreEmptyAndSpecialStrings = true)
        {
            var list = new List<IOneSmaliString>();

            _fileLines = File.ReadAllLines(FileName, FileEncoding);

            for (int i = 0; i < _fileLines.Length; i++)
            {
                string line = _fileLines[i];

                foreach (string[] value in SmaliSides)
                {
                    //string startItem = value[0];

                    int index = line.IndexOf(value[0], StringComparison.Ordinal);    // >const-string v2, "test \" yee"

                    if (index == -1)
                        continue;

                    index = line.IndexOf(value[1], index, StringComparison.Ordinal); // const-string v2>, "test \" yee"
                    index++;                        // const-string v2,> "test \" yee"
                    index += SpaceCountAfterComma;  // const-string v2, >"test \" yee"
                    index++;                        // const-string v2, ">test \" yee"

                    int endIndex = line.IndexOf('"', index);    // const-string v2, "test \>" yee"

                    if (endIndex == -1)
                        continue;

                    while (line[endIndex - 1] == '\\' && line[endIndex - 2] != '\\')
                    {
                        endIndex = line.IndexOf('"', endIndex + 1);     // const-string v2, "test \" yee>"

                        if (endIndex == -1)
                            break;
                    }

                    if (endIndex == -1)
                        continue;

                    int count = endIndex - index; // const-string v2, "->test \" yee<-"
                    var chars = new char[count];
                    line.CopyTo(index, chars, 0, count);
                    string text = new string(chars);

                    if (ignoreEmptyAndSpecialStrings)
                    {
                        if (string.IsNullOrWhiteSpace(text) || ShortStrings.Contains(text.Trim()))
                            continue;
                    }

                    list.Add(new OneSmaliString(value[0], text, index, i));

                    break;
                }
            }

            _details = new ReadOnlyObservableCollection<IOneString>(new ObservableCollection<IOneString>(list));
        }

        /*
        /// <summary>
        /// Загружает строки из файла на диске
        /// </summary>
        public override void LoadStrings()
        {
            ClearItems();
            stringsLoaded = true;

            var lst = new List<OneSmaliString>();
            textOfFile = File.ReadAllText(FileName, new UTF8Encoding(false));
            var constStringsList = new[] { "const-string v", "," };
            var finalStringsList = new[] { ":Ljava/lang/String;", "=" };
            var valuesList = new[] { constStringsList, finalStringsList };
            foreach (var list in valuesList)
            {
                int index = textOfFile.IndexOf(list[0], StringComparison.Ordinal); // >const-string v2, "test \" yee"
                while (index > -1)
                {
                    int temp = index;
                    index = textOfFile.IndexOf(list[1], index, StringComparison.Ordinal); // const-string v2>, "test \" yee"
                    index += 2; // const-string v2, >"test \" yee"
                    index++; // const-string v2, ">test \" yee"

                    if (index == -1 || index - temp - list[0].Length > 10 || textOfFile[index - 1] != '\"')
                    {
                        index = textOfFile.IndexOf(list[0], temp + list[0].Length, StringComparison.Ordinal);
                        continue;
                    }

                    int endIndex = textOfFile.IndexOf('"', index); // const-string v2, "test \>" yee"
                    if (endIndex == -1)
                    {
                        index = textOfFile.IndexOf(list[0], index, StringComparison.Ordinal);
                        continue;
                    }
                    while (textOfFile[endIndex - 1] == '\\' && textOfFile[endIndex - 2] != '\\')
                    {
                        endIndex = textOfFile.IndexOf('"', endIndex + 1); // const-string v2, "test \" yee>"
                        if (endIndex < 0) goto end;
                    }
                    int count = endIndex - index; // const-string v2, "->test \" yee<-"
                    var chars = new char[count];
                    textOfFile.CopyTo(index, chars, 0, count);
                    string text = new string(chars);
                    string trimmed = text.Trim();
                    var incStrings = new[] { "[", "]", ",", ":", "{", "}", "=", "<", ">" };
                    if (string.IsNullOrWhiteSpace(text) || incStrings.Any(str => str == trimmed))
                        goto end;

                    lst.Add(new OneSmaliString(list[0], text, index));
                    end:

                    index = textOfFile.IndexOf(list[0], endIndex, StringComparison.Ordinal);
                }
            }

            lst = lst.OrderBy(i => i.StartIndex).ToList();

            foreach (var item in lst)
                Add(item);
        }*/

        /*/// <summary>
        /// Заменяет последовательности Unicode символов в строках
        /// </summary>
        public void ReplaceSequences()
        {
            for (int i = 0; i < Count; i++)
            {
                OneSmaliString str = this[i].As<OneSmaliString>();
                string replaced = UnicodeStringToNET(str.OldText);
                //string replaced = Regex.Unescape(str.OldText).Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\"", "\\\"").Replace("'", "\\'");
                if (str.OldText != replaced)
                {
                    str.NewText = replaced;
                    textOfFile = str.SaveChanges(textOfFile, this, i);
                }
            }

            //File.WriteAllText(FileName, textOfFile, new UTF8Encoding(false));
        }*/
    }
}
