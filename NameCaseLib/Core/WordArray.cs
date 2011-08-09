using NameCaseLib.NCL;
using System;
namespace NameCaseLib.Core
{
    /// <summary>
    /// Класс для создания динамического массива слов
    /// </summary>
    public class WordArray
    {
        private int length = 0;
        private int capacity = 4;

        private Word[] words;

        /// <summary>
        /// Создаем новый массив слов со стандартной длиной
        /// </summary>
        public WordArray()
        {
            words = new Word[capacity];
        }
        
        /// <summary>
        /// Получаем из массива слов слово с указаным индексом
        /// </summary>
        /// <param name="id">Индекс слова</param>
        /// <returns>Слово</returns>
        public Word GetWord(int id)
        {
            return words[id];
        }

        private void EnlargeArray()
        {
            Word[] tmp = new Word[capacity * 2];
            Array.Copy(words, tmp, length);
            words = tmp;
            capacity *= 2;
        }

        /// <summary>
        /// Добавляем в массив слов новое слово
        /// </summary>
        /// <param name="word">Слово</param>
        public void AddWord(Word word)
        {
            if (length >= capacity)
            {
                EnlargeArray();
            }
            words[length] = word;
            length++;
        }

        /// <summary>
        /// Вовращает количество слов в массиве
        /// </summary>
        public int Length
        {
            get
            {
                return length;
            }
        }

        /// <summary>
        /// Находит имя/фамилию/отчество среди слов в массиве
        /// </summary>
        /// <param name="namePart">Что нужно найти</param>
        /// <returns>Слово</returns>
        public Word GetByNamePart(NamePart namePart)
        {
            for (int i = 0; i < length; i++)
            {
                if (words[i].NamePart == namePart)
                {
                    return words[i];
                }
            }
            return new Word("");
        }
    }
}
