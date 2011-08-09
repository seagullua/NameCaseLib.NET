using NameCaseLib.NCL;
using System;
namespace NameCaseLib.Core
{
    class WordArray
    {
        private int length = 0;
        private int capacity = 4;

        private Word[] words;

        public WordArray()
        {
            words = new Word[capacity];
        }

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

        public void AddWord(Word word)
        {
            if (length >= capacity)
            {
                EnlargeArray();
            }
            words[length] = word;
            length++;
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

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
