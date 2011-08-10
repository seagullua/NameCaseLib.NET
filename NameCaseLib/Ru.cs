using NameCaseLib.NCL;
using NameCaseLib.Core;
using System;
using System.Collections.Generic;
namespace NameCaseLib
{
    /// <summary>
    /// Русские правила склонения ФИО
    /// Правила определения пола человека по ФИО для русского языка
    /// Система разделения фамилий имен и отчеств для русского языка
    /// </summary>
    public class Ru : NameCaseLib.Core.Core
    {
        /// <summary>
        /// Версия языкового файла
        /// </summary>
        protected new static String languageBuild = "11072716";

        /// <summary>
        /// Количество падежей в языке
        /// </summary>
        protected new int caseCount = 6;

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
        /// Создание текущего объекта
        /// </summary>
        public Ru()
        {
            base.caseCount = this.caseCount;
        }

        private Dictionary<String, String> splitSecondExclude = new Dictionary<String, String>()
        {
            {"а", "взйкмнпрстфя"},
            {"б", "а"},
            {"в", "аь"},
            {"г", "а"},
            {"д", "ар"},
            {"е", "бвгдйлмня"},
            {"ё", "бвгдйлмня"},
            {"ж", ""},
            {"з", "а"},
            {"и", "гдйклмнопрсфя"},
            {"й", "ля"},
            {"к", "аст"},
            {"л", "аилоья"},
            {"м", "аип"},
            {"н", "ат"},
            {"о", "вдлнпртя"},
            {"п", "п"},
            {"р", "адикпть"},
            {"с", "атуя"},
            {"т", "аор"},
            {"у", "дмр"},
            {"ф", "аь"},
            {"х", "а"},
            {"ц", "а"},
            {"ч", ""},
            {"ш", "а"},
            {"щ", ""},
            {"ъ", ""},
            {"ы", "дн"},
            {"ь", "я"},
            {"э", ""},
            {"ю", ""},
            {"я", "нс"}
        };


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
                if (InNames(workingWord, "павел"))
                {
                    lastResult = new String [] {"павел", "павла", "павлу", "павла", "павлом", "павле"};
                    Rule(201);
                    return true;
                }
                else if (InNames(workingWord, "лев"))
                {
                    lastResult = new String [] {"лев", "льва", "льву", "льва", "львом", "льве"};
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

        /// <summary>
        /// Функция пытается применить цепочку правил для мужских имен
        /// @return boolean true - если было использовано правило из списка, false - если правило не было найденым
        /// </summary>
        protected override bool ManFirstName()
        {
            return RulesChain(Gender.Man, new int[]{1, 2, 3});
        }

        /// <summary>
        /// Функция пытается применить цепочку правил для женских имен
        /// @return boolean true - если было использовано правило из списка, false - если правило не было найденым
        /// </summary>
        protected override bool WomanFirstName()
        {
            return RulesChain(Gender.Woman, new int[]{1, 2, 3});
        }

        /// <summary>
        /// Функция пытается применить цепочку правил для мужских фамилий
        /// @return boolean true - если было использовано правило из списка, false - если правило не было найденым
        /// </summary>
        protected override bool ManSecondName()
        {
            return RulesChain(Gender.Man,  new int[]{8, 4, 5, 6, 7});
        }

        /// <summary>
        /// Функция пытается применить цепочку правил для женских фамилий
        /// @return boolean true - если было использовано правило из списка, false - если правило не было найденым
        /// </summary>
        protected override bool WomanSecondName()
        {
            return RulesChain(Gender.Woman,  new int[]{4});
        }

        /// <summary>
        /// Функция склоняет мужский отчества
        /// @return boolean true - если слово было успешно изменено, false - если не получилось этого сделать
        /// </summary>
        protected override bool ManFatherName()
        {
            //Проверяем действительно ли отчество
            if (InNames(workingWord, "ильич"))
            {
                WordForms(workingWord, new String [] {"а", "у", "а", "ом", "е"});
                return true;
            }
            else if (Last(2) == "ич")
            {
                WordForms(workingWord, new String [] {"а", "у", "а", "ем", "е"});
                return true;
            }
            return false;
        }

        /// <summary>
        /// Функция склоняет женские отчества
        /// @return boolean true - если слово было успешно изменено, false - если не получилось этого сделать
        /// </summary>
        protected override bool WomanFatherName()
        {
            //Проверяем действительно ли отчество
            if (Last(2) == "на")
            {
                WordForms(workingWord, new String [] {"ы", "е", "у", "ой", "е"}, 1);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Определение пола по правилам имен
        /// @param NCLNameCaseWord word обьект класса слов, для которого нужно определить пол
        /// </summary>
        protected override void GenderByFirstName(Word word)
        {
            SetWorkingWord(word.Name);

            GenderProbability prob = new GenderProbability();
            
            //Попробуем выжать максимум из имени
            //Если имя заканчивается на й, то скорее всего мужчина
            if (Last(1) == "й")
            {
                prob.Man+=0.9f;
            }
            if (In(Last(2), new String [] {"он", "ов", "ав", "ам", "ол", "ан", "рд", "мп"}))
            {
                prob.Man+=0.3f;
            }
            if (In(Last(1), consonant))
            {
                prob.Man+=0.01f;
            }
            if (Last(1) == "ь")
            {
                prob.Man+=0.02f;
            }

            if (In(Last(2), new String [] {"вь", "фь", "ль"}))
            {
                prob.Woman+=0.1f;
            }

            if (In(Last(2), new String [] {"ла"}))
            {
                prob.Woman+=0.04f;
            }

            if (In(Last(2), new String [] {"то", "ма"}))
            {
                prob.Man+=0.01f;
            }

            if (In(Last(3), new String [] {"лья", "вва", "ока", "ука", "ита"}))
            {
                prob.Man+=0.2f;
            }

            if (In(Last(3), new String [] {"има"}))
            {
                prob.Woman+=0.15f;
            }

            if (In(Last(3), new String [] {"лия", "ния", "сия", "дра", "лла", "кла", "опа"}))
            {
                prob.Woman+=0.5f;
            }

            if (In(Last(4), new String [] {"льда", "фира", "нина", "лита", "алья"}))
            {
                prob.Woman+=0.5f;
            }

            word.GenderProbability = prob;
        }

        /// <summary>
        /// Определение пола по правилам фамилий
        /// @param NCLNameCaseWord word обьект класса слов, для которого нужно определить пол
        /// </summary>
        protected override void GenderBySecondName(Word word)
        {
            SetWorkingWord(word.Name);

            GenderProbability prob = new GenderProbability();

            if (In(Last(2), new String [] {"ов", "ин", "ев", "ий", "ёв", "ый", "ын", "ой"}))
            {
                prob.Man+=0.4f;
            }

            if (In(Last(3), new String [] {"ова", "ина", "ева", "ёва", "ына", "мин"}))
            {
                prob.Woman+=0.4f;
            }

            if (In(Last(2), new String [] {"ая"}))
            {
                prob.Woman+=0.4f;
            }

            word.GenderProbability = prob;
        }

        /// <summary>
        /// Определение пола по правилам отчеств
        /// @param NCLNameCaseWord word обьект класса слов, для которого нужно определить пол
        /// </summary>
        protected override void GenderByFatherName(Word word)
        {
            SetWorkingWord(word.Name);


            if (Last(2) == "ич")
            {
                word.GenderProbability = new GenderProbability(10, 0); // мужчина
            }
            if (Last(2) == "на")
            {
                word.GenderProbability = new GenderProbability(0, 12); // женщина
            }
        }

        /// <summary>
        /// Идетифицирует слово определяе имя это, или фамилия, или отчество 
        /// - <b>N</b> - имя
        /// - <b>S</b> - фамилия
        /// - <b>F</b> - отчество
        /// @param NCLNameCaseWord word обьект класса слов, который необходимо идентифицировать
        /// </summary>
        protected override void DetectNamePart(Word word)
        {
            String name = word.Name;
            int length = name.Length;
            SetWorkingWord(name);

            //Считаем вероятность
            float first = 0;
            float second = 0;
            float father = 0;

            //если смахивает на отчество
            if (In(Last(3), new String [] {"вна", "чна", "вич", "ьич"}))
            {
                father+=3;
            }

            if (In(Last(2), new String [] {"ша"}))
            {
                first+=0.5f;
            }

            
            // буквы на которые никогда не закнчиваются имена
            if (In(Last(1), "еёжхцочшщъыэю"))
            {
                second += 0.3f;
            }

            
            // Используем массив характерных окончаний
            if (In(Last(2, 1), vowels+consonant))
            {
                if (!In(Last(1), splitSecondExclude[Last(2, 1)]))
                {
                    second += 0.4f;
                }
            }

            
            // Сохкращенные ласкательные имена типя Аня Галя и.т.д.
            if (Last(1) == "я" && In(Last(3, 1), vowels))
            {
                first += 0.5f;
            }

            
            // Не бывает имет с такими предпоследними буквами
            if (In(Last(2, 1), "жчщъэю"))
            {
                second += 0.3f;
            }

            
            // Слова на мягкий знак. Существует очень мало имен на мягкий знак. Все остальное фамилии
            if (Last(1) == "ь")
            {
                
                // Имена типа нинЕЛь адЕЛь асЕЛь
                if (Last(3, 2) == "ел")
                {
                    first += 0.7f;
                }
                
                // Просто исключения
                else if (InNames(name, new String [] {"лазарь", "игорь", "любовь"}))
                {
                    first += 10;
                }
                
                // Если не то и не другое, тогда фамилия
                else
                {
                    second += 0.3f;
                }
            }
            
            // Если две последних букв согласные то скорее всего это фамилия
            else if (In(Last(1), consonant + "ь") && In(Last(2, 1), consonant + "ь"))
            {
                
                // Практически все кроме тех которые оканчиваются на следующие буквы
                if (!In(Last(2), new String [] {"др", "кт", "лл", "пп", "рд", "рк", "рп", "рт", "тр"}))
                {
                    second += 0.25f;
                }
            }

            
            // Слова, которые заканчиваются на тин
            if (Last(3) == "тин" && In(Last(4, 1), "нст"))
            {
                first += 0.5f;
            }

            //Исключения
            if (InNames(name, new String [] {"лев", "яков", "маша", "ольга", "еремей", "исак", "исаак", "ева", "ирина", "элькин", "мерлин"}))
            {
                first+=10;
            }



            
            // Фамилии которые заканчиваются на -ли кроме тех что типа натАли и.т.д.
            if (Last(2) == "ли" && Last(3, 1) != "а")
            {
                second+=0.4f;
            }

            
            // Фамилии на -як кроме тех что типа Касьян Куприян + Ян и.т.д.
            if (Last(2) == "ян" && length > 2 && !In(Last(3, 1), "ьи"))
            {
                second+=0.4f;
            }

            
            // Фамилии на -ур кроме имен Артур Тимур
            if (Last(2) == "ур")
            {
                if (!InNames(name, new String [] {"артур", "тимур"}))
                {
                    second += 0.4f;
                }
            }

            
            // Разбор ласкательных имен на -ик
            if (Last(2) == "ик")
            {
                
                // Ласкательные буквы перед ик
                if (In(Last(3, 1), "лшхд"))
                {
                    first += 0.3f;
                }
                else
                {
                    second += 0.4f;
                }
            }

            
            // Разбор имен и фамилий, который заканчиваются на ина
            if (Last(3) == "ина")
            {
                
                // Все похожие на Катерина и Кристина
                if (In(Last(7), new String [] {"атерина", "ристина"}))
                {
                    first+=10;
                }
                
                // Исключения
                else if (InNames(name, new String [] {"мальвина", "антонина", "альбина", "агриппина", "фаина", "карина", "марина", "валентина", "калина", "аделина", "алина", "ангелина", "галина", "каролина", "павлина", "полина", "элина", "мина", "нина"}))
                {
                    first+=10;
                }
                
                // Иначе фамилия
                else
                {
                    second += 0.4f;
                }
            }

            
            // Имена типа Николай
            if (Last(4) == "олай")
            {
                first += 0.6f;
            }

            
            // Фамильные окончания
            if (In(Last(2), new String [] {"ов", "ин", "ев", "ёв", "ый", "ын", "ой", "ук", "як", "ца", "ун", "ок", "ая", "га", "ёк", "ив", "ус", "ак", "яр", "уз", "ах", "ай"}))
            {
                second+=0.4f;
            }

            if (In(Last(3), new String [] {"ова", "ева", "ёва", "ына", "шен", "мей", "вка", "шир", "бан", "чий", "кий", "бей", "чан", "ган", "ким", "кан", "мар"}))
            {
                second+=0.4f;
            }

            if (In(Last(4), new String [] {"шена"}))
            {
                second+=0.4f;
            }


            float max = Math.Max(Math.Max(first, second), father);

            if (first == max)
            {
                word.NamePart = NamePart.FirstName;
            }
            else if (second == max)
            {
                word.NamePart = NamePart.SecondName;
            }
            else
            {
                word.NamePart = NamePart.FatherName;
            }
        }
    }
}
