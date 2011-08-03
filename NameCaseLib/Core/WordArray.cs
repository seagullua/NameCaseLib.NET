using System;
using System.Collections.Generic;
using System.Text;

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

        public Word getWord(int id)
        {
            return words[id];
        }

        private void enlargeArray()
        {
            Word[] tmp = new Word[capacity * 2];
            Array.Copy(words, tmp, length);
            words = tmp;
            capacity *= 2;
        }

        public void addWord(Word word)
        {
            if (length >= capacity)
            {
                enlargeArray();
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
    }
}
