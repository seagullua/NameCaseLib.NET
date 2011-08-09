using NameCaseLib.NCL;
using NameCaseLib.Core;
using System;
namespace NameCaseLib
{
    public class Ru : NameCaseLib.Core.Core
    {
        /// <summary>
        /// Версия языкового файла
        /// </summary>
        protected static String languageBuild = "11072716";

        /// <summary>
        /// Количество падежей в языке
        /// </summary>
        protected int caseCount = 6;

        /// <summary>
        /// Список гласных русского языка
        /// </summary>
        private String vowels = "аеёиоуыэюя";

        /// <summary>
        /// Список согласных русского языка
        /// </summary>
        private String consonant = "бвгджзйклмнпрстфхцчшщ";

        /// <summary>
        /// Окончания имен/фамилий, который не склоняются
        /// </summary>
        private String[] ovo = new String[] {"ово", "аго", "яго", "ирь"};

        /// <summary>
        /// Окончания имен/фамилий, который не склоняются
        /// </summary>
        private String[] ih = new String[] {"их", "ых", "ко"};

        /// <summary>
        /// Мужские имена, оканчивающиеся на любой ь и -й, 
        /// скло­няются так же, как обычные существительные мужского рода
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool ManRule1()
        {
            if (In(Last(1), "ьй"))
            {
                if (Last(2, 1) != "и")
                {
                    WordForms(workingWord, new String [] {"я", "ю", "я", "ем", "е"}, 1);
                    Rule(101);
                    return true;
                }
                else
                {
                    WordForms(workingWord, new String [] {"я", "ю", "я", "ем", "и"}, 1);
                    Rule(102);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Мужские имена, оканчивающиеся на любой твердый согласный, 
        /// склоняются так же, как обычные существительные мужского рода
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool ManRule2()
        {
            if (In(Last(1), consonant))
            {
                if (InNames(workingWord, "Павел"))
                {
                    lastResult = new String [] {"Павел", "Павла", "Павлу", "Павла", "Павлом", "Павле"};
                    Rule(201);
                    return true;
                }
                else if (InNames(workingWord, "Лев"))
                {
                    lastResult = new String [] {"Лев", "Льва", "Льву", "Льва", "Львом", "Льве"};
                    Rule(202);
                    return true;
                }
                else
                {
                    WordForms(workingWord, new String [] {"а", "у", "а", "ом", "е"});
                    Rule(203);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Мужские и женские имена, оканчивающиеся на -а, склоняются, как и любые 
        /// существительные с таким же окончанием
        /// Мужские и женские имена, оканчивающиеся иа -я, -ья, -ия, -ея, независимо от языка, 
        /// из которого они происходят, склоняются как существительные с соответствующими окончаниями
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool ManRule3()
        {
            if (Last(1) == "а")
            {
                if (!In(Last(2, 1), "кшгх"))
                {
                    WordForms(workingWord, new String [] {"ы", "е", "у", "ой", "е"}, 1);
                    Rule(301);
                    return true;
                }
                else
                {
                    WordForms(workingWord, new String [] {"и", "е", "у", "ой", "е"}, 1);
                    Rule(302);
                    return true;
                }
            }
            else if (Last(1) == "я")
            {
                WordForms(workingWord, new String [] {"и", "е", "ю", "ей", "е"}, 1);
                Rule(303);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Мужские фамилии, оканчивающиеся на -ь -й, склоняются так же, 
        /// как обычные существительные мужского рода
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool ManRule4()
        {
            if (In(Last(1), "ьй"))
            {
                //Слова типа Воробей
                if (Last(3) == "бей")
                {
                    WordForms(workingWord, new String [] {"ья", "ью", "ья", "ьем", "ье"}, 2);
                    Rule(400);
                    return true;
                }
                else if (Last(3, 1) == "а" || In(Last(2, 1), "ел"))
                {
                    WordForms(workingWord, new String [] {"я", "ю", "я", "ем", "е"}, 1);
                    Rule(401);
                    return true;
                }
                //Толстой -» ТолстЫм 
                else if (Last(2, 1) == "ы" || Last(3, 1) == "т")
                {
                    WordForms(workingWord, new String [] {"ого", "ому", "ого", "ым", "ом"}, 2);
                    Rule(402);
                    return true;
                }
                //Лесничий
                else if (Last(3) == "чий")
                {
                    WordForms(workingWord, new String [] {"ьего", "ьему", "ьего", "ьим", "ьем"}, 2);
                    Rule(403);
                    return true;
                }
                else if (!In(Last(2, 1), vowels) || Last(2, 1) == "и")
                {
                    WordForms(workingWord, new String [] {"ого", "ому", "ого", "им", "ом"}, 2);
                    Rule(404);
                    return true;
                }
                else
                {
                    MakeResultTheSame();
                    Rule(405);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Мужские фамилии, оканчивающиеся на -к
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool ManRule5()
        {
            if (Last(1) == "к")
            {
                //Если перед слово на ок, то нужно убрать о
                if (Last(2, 1) == "о")
                {
                    WordForms(workingWord, new String [] {"ка", "ку", "ка", "ком", "ке"}, 2);
                    Rule(501);
                    return true;
                }
                if (Last(2, 1) == "е")
                {
                    WordForms(workingWord, new String [] {"ька", "ьку", "ька", "ьком", "ьке"}, 2);
                    Rule(502);
                    return true;
                }
                else
                {
                    WordForms(workingWord, new String [] {"а", "у", "а", "ом", "е"});
                    Rule(503);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Мужские фамили на согласный выбираем ем/ом/ым
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool ManRule6()
        {
            if (Last(1) == "ч")
            {
                WordForms(workingWord, new String [] {"а", "у", "а", "ем", "е"});
                Rule(601);
                return true;
            }
            //е перед ц выпадает
            else if (Last(2) == "ец")
            {
                WordForms(workingWord, new String [] {"ца", "цу", "ца", "цом", "це"}, 2);
                Rule(604);
                return true;
            }
            else if (In(Last(1), "цсршмхт"))
            {
                WordForms(workingWord, new String [] {"а", "у", "а", "ом", "е"});
                Rule(602);
                return true;
            }
            else if (In(Last(1), consonant))
            {
                WordForms(workingWord, new String [] {"а", "у", "а", "ым", "е"});
                Rule(603);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Мужские фамили на -а -я
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns> 
        protected bool ManRule7()
        {
            if (Last(1) == "а")
            {
                //Если основа на ш, то нужно и, ей
                if (Last(2, 1) == "ш")
                {
                    WordForms(workingWord, new String [] {"и", "е", "у", "ей", "е"}, 1);
                    Rule(701);
                    return true;
                }
                else if (In(Last(2, 1), "хкг"))
                {
                    WordForms(workingWord, new String [] {"и", "е", "у", "ой", "е"}, 1);
                    Rule(702);
                    return true;
                }
                else
                {
                    WordForms(workingWord, new String [] {"ы", "е", "у", "ой", "е"}, 1);
                    Rule(703);
                    return true;
                }
            }
            else if (Last(1) == "я")
            {
                WordForms(workingWord, new String [] {"ой", "ой", "ую", "ой", "ой"}, 2);
                Rule(704);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Не склоняются мужский фамилии
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns> 
        protected bool ManRule8()
        {
            if (In(Last(3), ovo) || In(Last(2), ih))
            {
                Rule(8);
                MakeResultTheSame();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Мужские и женские имена, оканчивающиеся на -а, склоняются, 
        /// как и любые существительные с таким же окончанием
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool WomanRule1()
        {
            if (Last(1) == "а" && Last(2, 1) != "и")
            {
                if (!In(Last(2, 1), "шхкг"))
                {
                    WordForms(workingWord, new String [] {"ы", "е", "у", "ой", "е"}, 1);
                    Rule(101);
                    return true;
                }
                else
                {
                    //ей посля шиплячего
                    if (Last(2, 1) == "ш")
                    {
                        WordForms(workingWord, new String [] {"и", "е", "у", "ей", "е"}, 1);
                        Rule(102);
                        return true;
                    }
                    else
                    {
                        WordForms(workingWord, new String [] {"и", "е", "у", "ой", "е"}, 1);
                        Rule(103);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Мужские и женские имена, оканчивающиеся иа -я, -ья, -ия, -ея, независимо от языка, 
        /// из которого они происходят, склоняются как сущест­вительные с соответствующими окончаниями
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns> 
        protected bool WomanRule2()
        {
            if (Last(1) == "я")
            {
                if (Last(2, 1) != "и")
                {
                    WordForms(workingWord, new String [] {"и", "е", "ю", "ей", "е"}, 1);
                    Rule(201);
                    return true;
                }
                else
                {
                    WordForms(workingWord, new String [] {"и", "и", "ю", "ей", "и"}, 1);
                    Rule(202);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Русские женские имена, оканчивающиеся на мягкий согласный, склоняются, 
        /// как существительные женского рода типа дочь, тень
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool WomanRule3()
        {
            if (Last(1) == "ь")
            {
                WordForms(workingWord, new String [] {"и", "и", "ь", "ью", "и"}, 1);
                Rule(3);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Женские фамилия, оканчивающиеся на -а -я, склоняются,
        /// как и любые существительные с таким же окончанием
        /// </summary>
        /// <returns>если правило было задействовано и false если нет</returns>
        protected bool WomanRule4()
        {

            if (Last(1) == "а")
            {
                if (In(Last(2, 1), "гк"))
                {
                    WordForms(workingWord, new String [] {"и", "е", "у", "ой", "е"}, 1);
                    Rule(401);
                    return true;
                }
                else if (In(Last(2, 1), "ш"))
                {
                    WordForms(workingWord, new String [] {"и", "е", "у", "ей", "е"}, 1);
                    Rule(402);
                    return true;
                }
                else
                {
                    WordForms(workingWord, new String [] {"ой", "ой", "у", "ой", "ой"}, 1);
                    Rule(403);
                    return true;
                }
            }
            else if (Last(1) == "я")
            {
                WordForms(workingWord, new String [] {"ой", "ой", "ую", "ой", "ой"}, 2);
                Rule(404);
                return true;
            }
            return false;
        }
    }
}
