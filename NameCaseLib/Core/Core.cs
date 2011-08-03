using System;
using System.Collections.Generic;
using System.Text;

namespace NameCaseLib.Core
{
    /// <summary>
    /// Набор основных функций, который позволяют сделать интерфейс слонения русского и украниского языка
    /// абсолютно одинаковым. Содержит все функции для внешнего взаимодействия с библиотекой.
    /// </summary>
    abstract class Core
    {
        /// <summary>
        /// Версия библиотеки
        /// </summary>
        private static String version = "0.0.1";

        /// <summary>
        /// Версия языкового файла
        /// </summary>
        protected static String languageBuild = "";

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
        private String workingWord;

        /// <summary>
        /// Метод Last() вырезает подстроки разной длины. Посколько одинаковых вызовов бывает несколько,
        /// то все результаты выполнения кешируются в этом массиве.
        /// </summary>
        private String[,] workindLastCache;

        /// <summary>
        /// Номер последнего использованого правила, устанавливается методом Rule()
        /// </summary>
        private int lastRule = 0;

        /// <summary>
        /// Массив содержит результат склонения слова - слово во всех падежах
        /// </summary>
        private String[] lastResult;

        /// <summary>
        /// Количество падежей в языке
        /// </summary>
        protected int caseCount = 0;

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
        /// Считывает и устанавливает последние прафило
        /// </summary>
        protected int Rule
        {
            set
            {
                lastRule = value;
            }
            get
            {
                return lastRule;
            }
        }
    }
}
