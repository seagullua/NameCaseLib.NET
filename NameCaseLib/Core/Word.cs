using System;
using NameCaseLib.NCL;

namespace NameCaseLib.Core
{
    /// <summary>
    /// Word - класс, который служит для хранения всей информации о каждом слове
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Слово в нижнем регистре, которое хранится в об’єкте класса
        /// </summary>
        private String word;

        /// <summary>
        /// Тип текущей записи (Фамилия/Имя/Отчество)
        /// </summary>
        private NamePart namePart = NamePart.Null;

        /// <summary>
        /// Вероятность того, что текущей слово относится к или женскому полу
        /// </summary>
        private GenderProbability manOrWoman;


        /// <summary>
        /// Окончательное решение, к какому полу относится слово
        /// </summary>
        private Gender genderSolved = Gender.Null;

        /// <summary>
        /// Маска больших букв в слове.
        /// 
        /// Содержит информацию о том, какие буквы в слове были большими, а какие мальникими:
        /// - x - маленькая буква
        /// - X - больная буква
        /// </summary>
        private LettersMask[] letterMask;

        /// <summary>
        /// Содержит true, если все слово было в верхнем регистре и false, если не было
        /// </summary>
        private bool isUpperCase = false;

        /// <summary>
        /// Массив содержит все падежи слова, полученые после склонения текущего слова
        /// </summary>
        private String[] nameCases;

        /// <summary>
        /// Номер правила, по которому было произведено склонение текущего слова
        /// </summary>
        private int rule = 0;

        /// <summary>
        /// Создание нового обьекта со словом
        /// </summary>
        /// <param name="word">Слово</param>
        public Word(String word)
        {
            GenerateMask(word);
            this.word = word.ToLower();
        }

        /// <summary>
        /// Генерирует маску, которая содержит информацию о том, какие буквы в слове были большими, а какие маленькими:
        /// - x - маленькая буква
        /// - Х - большая буква
        /// </summary>
        /// <param name="word">Слово для которого нужна маска</param>
        private void GenerateMask(String word)
        {
            isUpperCase = true;
            int length = word.Length;
            letterMask = new LettersMask[length];

            for (int i = 0; i < length; i++)
            {
                String letter = word.Substring(i, 1);
                if (Str.isLowerCase(letter))
                {
                    isUpperCase = false;
                    letterMask[i] = LettersMask.x;
                }
                else
                {
                    letterMask[i] = LettersMask.X;
                }
            }
        }

        /// <summary>
        /// Возвращает все падежи слова в начальную маску
        /// </summary>
        private void ReturnMask()
        {
            int wordCount = nameCases.Length;
            if (isUpperCase)
            {
                for (int i = 0; i < wordCount; i++)
                {
                    nameCases[i] = nameCases[i].ToUpper();
                }
            }
            else
            {
                for (int i = 0; i < wordCount; i++)
                {
                    int lettersCount = nameCases[i].Length;
                    int maskLength = letterMask.Length;
                    String newStr = "";
                    for (int letter = 0; letter < lettersCount; letter++)
                    {
                        if (letter < maskLength && letterMask[letter] == LettersMask.X)
                        {
                            newStr += nameCases[i].Substring(letter, 1).ToUpper();
                        }
                        else
                        {
                            newStr += nameCases[i].Substring(letter, 1);
                        }
                    }
                    nameCases[i] = newStr;
                }
            }
        }

        /// <summary>
        /// Считывает или устанавливает все падежи
        /// </summary>
        public String[] NameCases
        {
            set { 
                nameCases = value;
                ReturnMask();
            }
            get
            {
                return nameCases;
            }
        }

        /// <summary>
        /// Расчитывает и возвращает пол текущего слова. Или устанавливает нужный пол.
        /// </summary>
        public Gender Gender
        {
            get
            {
                if (genderSolved == Gender.Null)
                {
                    if (manOrWoman.Man > manOrWoman.Woman)
                    {
                        genderSolved = Gender.Man;
                    }
                    else
                    {
                        genderSolved = Gender.Woman;
                    }
                }
                return genderSolved;
            }
            set
            {
                genderSolved = value;
            }
        }



        /// <summary>
        /// Возвращает строку с нужным падежом текущего слова
        /// </summary>
        /// <param name="pageg">нужный падеж</param>
        /// <returns>строка с нужным падежом текущего слова</returns>
        public String GetNameCase(Padeg pageg)
        {
            return nameCases[(int)pageg];
        }

        /// <summary>
        /// Устанавливает вероятности того, что текущий челове мужчина или женщина
        /// </summary>
        public GenderProbability GenderProbability
        {
            get
            {
                return manOrWoman;
            }
            set
            {
                manOrWoman = value;
            }
        }

        /// <summary>
        /// Возвращает или устанавливает идентификатор части ФИО
        /// </summary>
        public NamePart NamePart
        {
            get
            {
                return namePart;
            }
            set
            {
                namePart = value;
            }
        }

        /// <summary>
        /// Текущее слово
        /// </summary>
        public String Name
        {
            get
            {
                return word;
            }
        }

        /// <summary>
        /// Если уже был расчитан пол для всех слов системы, тогда каждому слову предается окончательное
        /// решение. Эта функция определяет было ли принято окончательное решение.
        /// </summary>
        /// <returns>true если определен и false если нет</returns>
        public bool isGenderSolved()
        {
            if (genderSolved == Gender.Null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Устанавливает или считывает правило склонения текущего слова
        /// </summary>
        public int Rule
        {
            get
            {
                return rule;
            }
            set
            {
                rule = value;
            }
        }
    }
}
