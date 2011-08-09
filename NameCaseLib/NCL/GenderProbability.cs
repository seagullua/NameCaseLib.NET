using System;

namespace NameCaseLib.NCL
{
    /// <summary>
    /// Класс который содержит тип данных для определения пола человека
    /// </summary>
    public class GenderProbability
    {
        private float manProbability = 0;
        private float womanProbability = 0;

        /// <summary>
        /// Создать новый объект с указанием вероятности принадлежности пола мужчине или женщине
        /// </summary>
        /// <param name="man">Вероятноть мужского пола</param>
        /// <param name="woman">Вероятность женского пола</param>
        public GenderProbability(float man, float woman)
        {
            manProbability = man;
            womanProbability = woman;
        }
        
        /// <summary>
        /// Создание пустного объекта для подальшего накопления вероятностей
        /// </summary>
        public GenderProbability()
            : this(0, 0)
        {
        }

        /// <summary>
        /// Получить/Укзать вероятность мужского пола
        /// </summary>
        public float Man
        {
            get
            {
                return manProbability;
            }
            set
            {
                manProbability = value;
            }
        }

        /// <summary>
        /// Получить/Укзать вероятность женского пола
        /// </summary>
        public float Woman
        {
            get
            {
                return womanProbability;
            }
            set
            {
                womanProbability = value;
            }
        }

        /// <summary>
        /// Просумировать две вероятности
        /// </summary>
        /// <param name="number">Первая вероятность</param>
        /// <param name="add">Вторая вероятность</param>
        /// <returns>Сумма вероятностей</returns>
        static public GenderProbability operator+(GenderProbability number, GenderProbability add)
        {
            GenderProbability result = new GenderProbability(0, 0);
            result.Man = number.Man + add.Man;
            result.Woman = number.Woman + add.Woman;
            return result;
        }
    }
}
