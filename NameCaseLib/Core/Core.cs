using System;
using System.Reflection;
using NameCaseLib.NCL;
namespace NameCaseLib.Core
{
    /// <summary>
    /// Набор основных функций, который позволяют сделать интерфейс слонения русского и украниского языка
    /// абсолютно одинаковым. Содержит все функции для внешнего взаимодействия с библиотекой.
    /// </summary>
    public abstract class Core
    {
        /// <summary>
        /// Версия библиотеки
        /// </summary>
        private static String version = "0.0.1";

        /// <summary>
        /// Версия языкового файла
        /// </summary>
        protected static String languageBuild;

        /// <summary>
        /// Готовность системы:
        /// - Все слова идентифицированы (известо к какой части ФИО относится слово)
        /// - У всех слов определен пол
        /// Если все сделано стоит флаг true, при добавлении нового слова флаг сбрасывается на false
        /// </summary>
        private bool ready = false;

        /// <summary>
        /// Если все текущие слова было просклонены и в каждом слове уже есть результат склонения,
        /// тогда true. Если было добавлено новое слово флаг збрасывается на false
        /// </summary>
        private bool finished = false;

        /// <summary>
        ///  Массив содержит елементы типа Word. Это все слова которые нужно обработать и просклонять
        /// </summary>
        private WordArray words;

        /// <summary>
        /// Переменная, в которую заносится слово с которым сейчас идет работа
        /// </summary>
        protected String workingWord;

        /// <summary>
        /// Метод Last() вырезает подстроки разной длины. Посколько одинаковых вызовов бывает несколько,
        /// то все результаты выполнения кешируются в этом массиве.
        /// </summary>
        private LastCache workindLastCache;

        /// <summary>
        /// Номер последнего использованого правила, устанавливается методом Rule()
        /// </summary>
        private int lastRule = 0;

        /// <summary>
        /// Массив содержит результат склонения слова - слово во всех падежах
        /// </summary>
        protected String[] lastResult;

        /// <summary>
        /// Количество падежей в языке
        /// </summary>
        protected int caseCount;

        /// <summary>
        /// Метод очищает результаты последнего склонения слова. Нужен при склонении нескольких слов.
        /// </summary>
        private void Reset()
        {
            lastRule = 0;
            lastResult = new String[caseCount];
        }

        /// <summary>
        /// Устанавливает флаги о том, что система не готово и слова еще не были просклонены
        /// </summary>
        private void NotReady()
        {
            ready = false;
            finished = false;
        }

        /// <summary>
        /// Сбрасывает все информацию на начальную. Очищает все слова добавленые в систему.
        /// После выполнения система готова работать с начала. 
        /// </summary>
        public Core FullReset()
        {
            words = new WordArray();
            Reset();
            NotReady();
            return this;
        }

        /// <summary>
        /// Устанавливает последнее правило
        /// </summary>
        /// <param name="ruleID">Правило</param>
        protected void Rule(int ruleID)
        {
            lastRule = ruleID;
        }

        /// <summary>
        /// Считывает и устанавливает последние правило
        /// </summary>
        protected int GetRule()
        {
            return lastRule;
        }

        /// <summary>
        /// Устанавливает слово текущим для работы системы. Очищает кеш слова.
        /// </summary>
        /// <param name="word">слово, которое нужно установить</param>
        protected void SetWorkingWord(String word)
        {
            Reset();
            workingWord = word;
            workindLastCache = new LastCache();
        }

        /// <summary>
        /// Если не нужно склонять слово, делает результат таким же как и именительный падеж
        /// </summary>
        protected void MakeResultTheSame()
        {
            lastResult = new String[caseCount];
            for (int i = 0; i < caseCount; i++)
            {
                lastResult[i] = workingWord;
            }
        }

        /// <summary>
        /// Вырезает определенное количество последних букв текущего слова
        /// </summary>
        /// <param name="length">Количество букв</param>
        /// <returns>Подстроку содержущую определенное количество букв</returns>
        protected String Last(int length)
        {
            String result = workindLastCache.Get(length, length);
            if (result == "")
            {
                int startIndex = workingWord.Length - length;
                if (startIndex >= 0)
                {
                    result = workingWord.Substring(workingWord.Length - length, length);
                }
                else
                {
                    result = workingWord;  
                }
                workindLastCache.Push(result, length, length);
            }
            return result;
        }

        /// <summary>
        /// Вырезает stopAfter букв начиная с length с конца
        /// </summary>
        /// <param name="length">На сколько букв нужно оступить от конца</param>
        /// <param name="stopAfter">Сколько букв нужно вырезать</param>
        /// <returns>Искомоя строка</returns>
        protected String Last(int length, int stopAfter)
        {
            String result = workindLastCache.Get(length, stopAfter);
            if (result == "")
            {
                int startIndex = workingWord.Length - length;
                if (startIndex >= 0)
                {
                    result = workingWord.Substring(workingWord.Length - length, stopAfter);
                }
                else
                {
                    result = workingWord;
                }
                workindLastCache.Push(result, length, stopAfter);
            }
            return result;
        }

        /// <summary>
        /// Над текущим словом выполняются правила в указаном порядке.
        /// </summary>
        /// <param name="gender">Пол текущего слова</param>
        /// <param name="rulesArray">Порядок правил</param>
        /// <returns>Если правило было использовао true если нет тогда false</returns>
        protected bool RulesChain(Gender gender, int[] rulesArray)
        {
            if (gender != Gender.Null)
            {
                int rulesLength = rulesArray.Length;
                String rulesName = (gender == Gender.Man ? "Man" : "Woman");
                Type classType = this.GetType();
                for (int i = 0; i < rulesLength; i++)
                {
                    String methodName = String.Format("{0}Rule{1}", rulesName, rulesArray[i]);
                    bool res = (bool)classType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, null);
                    if (res)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет входит ли буква в список букв
        /// </summary>
        /// <param name="needle">буква</param>
        /// <param name="letters">список букв</param>
        /// <returns>true если входит в список и false если не входит</returns>
        protected bool In(String needle, String letters)
        {
            if (letters.IndexOf(needle) >= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ищет окончание в списке окончаний
        /// </summary>
        /// <param name="needle">окончание</param>
        /// <param name="haystack">список окончаний</param>
        /// <returns>true если найдено и false если нет</returns>
        protected bool In(String needle, String[] haystack)
        {
            int length = haystack.Length;
            for (int i = 0; i < length; i++)
            {
                if (haystack[i] == needle)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет равенство имени
        /// </summary>
        /// <param name="needle">имя</param>
        /// <param name="name">имя с которым сравнить</param>
        /// <returns>если имена совапали true</returns>
        protected bool InNames(String needle, String name)
        {
            if (needle == name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет входит ли имя в список имен
        /// </summary>
        /// <param name="needle">имя</param>
        /// <param name="names">список имен</param>
        /// <returns>true если входит</returns>
        protected bool InNames(String needle, String[] names)
        {
            int length = names.Length;
            for (int i = 0; i < length; i++)
            {
                if (needle == names[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Склоняем слово во все падежи используя окончания
        /// </summary>
        /// <param name="word">Слово</param>
        /// <param name="endings">окончания</param>
        /// <param name="replaceLast">сколько последних букв нужно убрать</param>
        protected void WordForms(String word, String[] endings, int replaceLast)
        {
            //Сохраняем именительный падеж
            lastResult = new String[caseCount];
            lastResult[0] = workingWord;

            //убираем лишние буквы
            word = word.Substring(0, word.Length - replaceLast);

            //Приписуем окончания
            for (int i = 1; i < caseCount; i++)
            {
                lastResult[i] = word + endings[i - 1];
            }
        }

        /// <summary>
        /// Создает список слов во всех падежах используя окончания для каждого падежа
        /// </summary>
        /// <param name="word">слово</param>
        /// <param name="endings">окончания</param>
        protected void WordForms(String word, String[] endings)
        {
            WordForms(word, endings, 0);
        }

        /// <summary>
        /// Установить имя человека
        /// </summary>
        /// <param name="name">Имя</param>
        /// <returns></returns>
        public Core SetFirstName(String name)
        {
            if (name.Trim() != "")
            {
                Word tmpWord = new Word(name);
                tmpWord.NamePart = NamePart.FirstName;
                words.AddWord(tmpWord);
                NotReady();
            }
            return this;
        }

        /// <summary>
        /// Установить фамилию человека
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <returns></returns>
        public Core SetSecondName(String name)
        {
            if (name.Trim() != "")
            {
                Word tmpWord = new Word(name);
                tmpWord.NamePart = NamePart.SecondName;
                words.AddWord(tmpWord);
                NotReady();
            }
            return this;
        }

        /// <summary>
        /// Установить отчество человека
        /// </summary>
        /// <param name="name">Отчество</param>
        /// <returns></returns>
        public Core SetFatherName(String name)
        {
            if (name.Trim() != "")
            {
                Word tmpWord = new Word(name);
                tmpWord.NamePart = NamePart.FatherName;
                words.AddWord(tmpWord);
                NotReady();
            }
            return this;
        }

        /// <summary>
        /// Устанавливает пол человека
        /// </summary>
        /// <param name="gender">Пол человека</param>
        /// <returns></returns>
        public Core SetGender(Gender gender)
        {
            int length = words.Length;
            for (int i = 0; i < length; i++)
            {
                words.GetWord(i).Gender = gender;
            }
            return this;
        }

        /// <summary>
        /// Устанавливает полное ФИО
        /// </summary>
        /// <param name="secondName">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="fatherName">Отчество</param>
        /// <returns></returns>
        public Core SetFullName(String secondName, String firstName, String fatherName)
        {
            SetFirstName(firstName);
            SetSecondName(secondName);
            SetFatherName(fatherName);
            return this;
        }

        /// <summary>
        /// Установить имя человека
        /// </summary>
        /// <param name="name">Имя</param>
        /// <returns></returns>
        public Core SetName(String name)
        {
            return SetFirstName(name);
        }

        /// <summary>
        /// Установить фамилию человека
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <returns></returns>
        public Core SetLastName(String name)
        {
            return SetSecondName(name);
        }

        /// <summary>
        /// Установить фамилию человека
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <returns></returns>
        public Core SetSirName(String name)
        {
            return SetSecondName(name);
        }



        /// <summary>
        /// Идентификирует нужное слово
        /// </summary>
        /// <param name="word">Слово</param>
        private void PrepareNamePart(Word word)
        {
            if (word.NamePart == NamePart.Null)
            {
                DetectNamePart(word);
            }
        }

        /// <summary>
        /// Идентифицирует все существующие слова
        /// </summary>
        private void PrepareAllNameParts()
        {
            int length = words.Length;
            for (int i = 0; i < length; i++)
            {
                PrepareNamePart(words.GetWord(i));
            }
        }

        /// <summary>
        /// Предварительно определяет пол во слове
        /// </summary>
        /// <param name="word">Слово для определения</param>
        private void PrepareGender(Word word)
        {
            if (!word.isGenderSolved())
            {
                switch (word.NamePart)
                {
                    case NamePart.FirstName: GenderByFirstName(word); break;
                    case NamePart.SecondName: GenderBySecondName(word); break;
                    case NamePart.FatherName: GenderByFatherName(word); break;
                }
            }
        }

        /// <summary>
        /// Принимает решение о текущем поле человека
        /// </summary>
        private void SolveGender()
        {
            //Ищем, может гдето пол уже установлен
            int length = words.Length;
            for (int i = 0; i < length; i++)
            {
                if (words.GetWord(i).isGenderSolved())
                {
                    SetGender(words.GetWord(i).Gender);
                    return;
                }
            }

            //Если нет тогда определяем у каждого слова и потом сумируем
            GenderProbability probability = new GenderProbability(0, 0);

            for (int i = 0; i < length; i++)
            {
                Word word = words.GetWord(i);
                PrepareGender(word);
                probability = probability + word.GenderProbability;
            }

            if (probability.Man > probability.Woman)
            {
                SetGender(Gender.Man);
            }
            else
            {
                SetGender(Gender.Woman);
            }
        }

        /// <summary>
        /// Выполнет все необходимые подготовления для склонения.
        /// Все слова идентфицируются. Определяется пол.
        /// </summary>
        private void PrepareEverything()
        {
            if (!ready)
            {
                PrepareAllNameParts();
                SolveGender();
                ready = true;
            }
        }

        /// <summary>
        /// По указаным словам определяется пол человека
        /// </summary>
        /// <returns>Пол человека</returns>
        public Gender GenderAutoDetect()
        {
            PrepareEverything();
            if (words.Length > 0)
            {
                return words.GetWord(0).Gender;
            }
            else
            {
                return Gender.Null;
            }
        }

        /// <summary>
        /// Разделяет слова на части и готовит к подальшуму склонению
        /// </summary>
        /// <param name="fullname">Строка котороя содержит полное имя</param>
        private void SplitFullName(String fullname)
        {
            String[] arr = fullname.Trim().Split(new Char[] {' '});
            int length = arr.Length;

            words = new WordArray();
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != "")
                {
                    words.AddWord(new Word(arr[i]));
                }
            }
        }

        /// <summary>
        /// Метод в разработке
        /// </summary>
        /// <returns></returns>
        public String GetFullNameFormat___DEV()
        {
            return "";
        }

        /// <summary>
        /// Склоняет слово по нужным правилам
        /// </summary>
        /// <param name="word">Слово</param>
        protected void WordCase(Word word)
        {
            Gender gender = word.Gender;
            String genderName = (gender == Gender.Man ? "Man" : "Woman");

            String namePartName = "";
            switch (word.NamePart)
            {
                case NamePart.FirstName: namePartName = "First"; break;
                case NamePart.SecondName: namePartName = "Second"; break;
                case NamePart.FatherName: namePartName = "Father"; break;
            }

            String methodName = genderName + namePartName + "Name";
            SetWorkingWord(word.Name);

            bool res = (bool)this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, null);
            if (res)
            {
                word.NameCases = lastResult;
                word.Rule = lastRule;
            }
            else
            {
                MakeResultTheSame();
                word.Rule = -1;
            }
        }

        /// <summary>
        /// Производит склонение всех слов
        /// </summary>
        private void AllWordCases()
        {
            if (!finished)
            {
                PrepareEverything();
                int length = words.Length;
                
                for (int i = 0; i < length; i++)
                {
                    WordCase(words.GetWord(i));
                }
                
                finished = true;
            }
        }

        /// <summary>
        /// Возвращает масив который содержит все падежи имени
        /// </summary>
        /// <returns>Возвращает массив со всеми падежами имени</returns>
        public String[] GetFirstNameCase()
        {
            AllWordCases();
            return words.GetByNamePart(NamePart.FirstName).NameCases;
        }

        /// <summary>
        /// Возвращает имя в определенном падеже
        /// </summary>
        /// <param name="caseNum">Падеж</param>
        /// <returns>Имя в определенном падеже</returns>
        public String GetFirstNameCase(Padeg caseNum)
        {
            AllWordCases();
            return words.GetByNamePart(NamePart.FirstName).GetNameCase(caseNum);
        }


        /// <summary>
        /// Возвращает масив который содержит все падежи фамилии
        /// </summary>
        /// <returns>Возвращает массив со всеми падежами фамилии</returns>
        public String[] GetSecondNameCase()
        {
            AllWordCases();
            return words.GetByNamePart(NamePart.SecondName).NameCases;
        }

        /// <summary>
        /// Возвращает фамилию в определенном падеже
        /// </summary>
        /// <param name="caseNum">Падеж</param>
        /// <returns>Фамилия в определенном падеже</returns>
        public String GetSecondNameCase(Padeg caseNum)
        {
            AllWordCases();
            return words.GetByNamePart(NamePart.SecondName).GetNameCase(caseNum);
        }

        /// <summary>
        /// Возвращает масив который содержит все падежи отчества
        /// </summary>
        /// <returns>Возвращает массив со всеми падежами отчества</returns>
        public String[] GetFatherNameCase()
        {
            AllWordCases();
            return words.GetByNamePart(NamePart.FatherName).NameCases;
        }

        /// <summary>
        /// Возвращает отчество в определенном падеже
        /// </summary>
        /// <param name="caseNum">Падеж</param>
        /// <returns>Отчество в определенном падеже</returns>
        public String GetFatherNameCase(Padeg caseNum)
        {
            AllWordCases();
            return words.GetByNamePart(NamePart.FatherName).GetNameCase(caseNum);
        }

        /// <summary>
        /// Выполняет склонение имени
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="gender">Пол</param>
        /// <returns>Массив со всеми падежами</returns>
        public String[] QFirstName(String firstName, Gender gender)
        {
            FullReset();
            SetFirstName(firstName);
            if (gender != Gender.Null)
            {
                SetGender(gender);
            }
            return GetFirstNameCase();
        }

        /// <summary>
        /// Выполняет склонение имени
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <returns>Массив со всеми падежами</returns>
        public String[] QFirstName(String firstName)
        {
            return QFirstName(firstName, Gender.Null);
        }

        /// <summary>
        /// Выполняет склонение имени
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="caseNum">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <returns>Имя в указаном падеже</returns>
        public String QFirstName(String firstName, Padeg caseNum, Gender gender)
        {
            FullReset();
            SetFirstName(firstName);
            if (gender != Gender.Null)
            {
                SetGender(gender);
            }
            return GetFirstNameCase(caseNum);
        }

        /// <summary>
        /// Выполняет склонение имени
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="caseNum">Падеж</param>
        /// <returns>Имя в указаном падеже</returns>
        public String QFirstName(String firstName, Padeg caseNum)
        {
            return QFirstName(firstName, caseNum, Gender.Null);
        }


        /// <summary>
        /// Выполняет склонение фамилии
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <param name="gender">Пол</param>
        /// <returns>Массив со всеми падежами</returns>
        public String[] QSecondName(String name, Gender gender)
        {
            FullReset();
            SetSecondName(name);
            if (gender != Gender.Null)
            {
                SetGender(gender);
            }
            return GetSecondNameCase();
        }

        /// <summary>
        /// Выполняет склонение фамилии
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <returns>Массив со всеми падежами</returns>
        public String[] QSecondName(String name)
        {
            return QSecondName(name, Gender.Null);
        }

        /// <summary>
        /// Выполняет склонение фамилии
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <param name="caseNum">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <returns>Фамилия в указаном падеже</returns>
        public String QSecondName(String name, Padeg caseNum, Gender gender)
        {
            FullReset();
            SetSecondName(name);
            if (gender != Gender.Null)
            {
                SetGender(gender);
            }
            return GetSecondNameCase(caseNum);
        }

        /// <summary>
        /// Выполняет склонение фамилии
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <param name="caseNum">Падеж</param>
        /// <returns>Фамилия в указаном падеже</returns>
        public String QSecondName(String name, Padeg caseNum)
        {
            return QSecondName(name, caseNum, Gender.Null);
        }

        /// <summary>
        /// Выполняет склонение фамилии
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <param name="gender">Пол</param>
        /// <returns>Массив со всеми падежами</returns>
        public String[] QFatherName(String name, Gender gender)
        {
            FullReset();
            SetFatherName(name);
            if (gender != Gender.Null)
            {
                SetGender(gender);
            }
            return GetFatherNameCase();
        }

        /// <summary>
        /// Выполняет склонение фамилии
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <returns>Массив со всеми падежами</returns>
        public String[] QFatherName(String name)
        {
            return QFatherName(name, Gender.Null);
        }

        /// <summary>
        /// Выполняет склонение фамилии
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <param name="caseNum">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <returns>Фамилия в указаном падеже</returns>
        public String QFatherName(String name, Padeg caseNum, Gender gender)
        {
            FullReset();
            SetFatherName(name);
            if (gender != Gender.Null)
            {
                SetGender(gender);
            }
            return GetFatherNameCase(caseNum);
        }

        /// <summary>
        /// Выполняет склонение фамилии
        /// </summary>
        /// <param name="name">Фамилия</param>
        /// <param name="caseNum">Падеж</param>
        /// <returns>Фамилия в указаном падеже</returns>
        public String QFatherName(String name, Padeg caseNum)
        {
            return QFatherName(name, caseNum, Gender.Null);
        }

        /// <summary>
        /// Соединяет все слова которые есть в системе в одну строку в определенном падеже
        /// </summary>
        /// <param name="caseNum">Падеж</param>
        /// <returns>Соедененая строка</returns>
        private String ConnectedCase(Padeg caseNum)
        {
            int length = words.Length;
            String result = "";
            for (int i = 0; i < length; i++)
            {
                result += words.GetWord(i).GetNameCase(caseNum) + " ";
            }
            return result.TrimEnd();
        }

        /// <summary>
        /// Соединяет все слова которые есть в системе в массив со всеми падежами
        /// </summary>
        /// <returns>Массив со всеми падежами</returns>
        private String[] ConnectedCases()
        {
            String[] res = new String[caseCount];
            for (int i = 0; i < caseCount; i++)
            {
                res[i] = ConnectedCase((Padeg)i);
            }

            return res;
        }

        /// <summary>
        /// Выполняет склонение полного имени
        /// </summary>
        /// <param name="fullName">Полное имя</param>
        /// <param name="gender">Пол</param>
        /// <returns>Массив со всеми падежами</returns>
        public String[] Q(String fullName, Gender gender)
        {
            FullReset();
            SplitFullName(fullName);
            if (gender != Gender.Null)
            {
                SetGender(gender);
            }
            AllWordCases();
            return ConnectedCases();
        }

        /// <summary>
        /// Выполняет склонение полного имени
        /// </summary>
        /// <param name="fullName">Полное имя</param>
        /// <returns>Массив со всеми падежами</returns>
        public String[] Q(String fullName)
        {
            return Q(fullName, Gender.Null);
        }

        /// <summary>
        /// Выполняет склонение полного имени
        /// </summary>
        /// <param name="fullName">Полное имя</param>
        /// <param name="caseNum">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <returns>Строка в указаном падеже</returns>
        public String Q(String fullName, Padeg caseNum, Gender gender)
        {
            FullReset();
            SplitFullName(fullName);
            if (gender != Gender.Null)
            {
                SetGender(gender);
            }
            AllWordCases();
            return ConnectedCase(caseNum);
        }

        /// <summary>
        /// Выполняет склонение полного имени
        /// </summary>
        /// <param name="fullName">Полное имя</param>
        /// <param name="caseNum">Падеж</param>
        /// <returns>Строка в указаном падеже</returns>
        public String Q(String fullName, Padeg caseNum)
        {
            return Q(fullName, caseNum, Gender.Null);
        }




        /// <summary>
        /// Возвращает массив всех слов
        /// </summary>
        /// <returns>Массив всех слов</returns>
        public WordArray GetWordsArray()
        {
            return words;
        }


        /// <summary>
        /// Склонение имени по правилам мужских имен
        /// </summary>
        /// <returns>true если склонение было произведено и false если правило не было найденым</returns>
        abstract protected bool ManFirstName();

        /// <summary>
        /// Склонение имени по правилам женских имен
        /// </summary>
        /// <returns>true если склонение было произведено и false если правило не было найденым</returns>
        abstract protected bool WomanFirstName();

        /// <summary>
        /// Склонение фамилию по правилам мужских имен
        /// </summary>
        /// <returns>true если склонение было произведено и false если правило не было найденым</returns>
        abstract protected bool ManSecondName();

        /// <summary>
        /// Склонение фамилию по правилам женских имен
        /// </summary>
        /// <returns>true если склонение было произведено и false если правило не было найденым</returns>
        abstract protected bool WomanSecondName();

        /// <summary>
        /// Склонение отчества по правилам мужских имен
        /// </summary>
        /// <returns>true если склонение было произведено и false если правило не было найденым</returns>
        abstract protected bool ManFatherName();

        /// <summary>
        /// Склонение отчества по правилам женских имен
        /// </summary>
        /// <returns>true если склонение было произведено и false если правило не было найденым</returns>
        abstract protected bool WomanFatherName();

        /// <summary>
        /// Определяет пол человека по его имени
        /// </summary>
        /// <param name="word">Имя</param>
        abstract protected void GenderByFirstName(Word word);


        /// <summary>
        /// Определяет пол человека по его фамилии
        /// </summary>
        /// <param name="word">Фамилия</param>
        abstract protected void GenderBySecondName(Word word);

        /// <summary>
        /// Определяет пол человека по его отчеству
        /// </summary>
        /// <param name="word">Отчество</param>
        abstract protected void GenderByFatherName(Word word);

        /// <summary>
        /// Идетифицирует слово определяе имя это, или фамилия, или отчество 
        /// </summary>
        /// <param name="word">Слово для которое нужно идетифицировать</param>
        abstract protected void DetectNamePart(Word word);  
        
        
        /// <summary>
        /// Возвращает текущую версию библиотке
        /// </summary>
        static public String Version
        {
            get
            {
                return version;
            }
        }

        /// <summary>
        /// Возвращает текущую версию языкового файла
        /// </summary>
        static public String LanguageVersion
        {
            get
            {
                return version;
            }
        }
    }
}
