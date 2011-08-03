using System;
using System.Collections.Generic;
using System.Text;

namespace NameCaseLib.NCL
{
    class GenderProbability
    {
        private float ManProbability = 0;
        private float WomanProbability = 0;


        public float Man
        {
            get
            {
                return ManProbability;
            }
            set
            {
                ManProbability = value;
            }
        }

        public float Woman
        {
            get
            {
                return WomanProbability;
            }
            set
            {
                WomanProbability = value;
            }
        }

        static public GenderProbability operator+(GenderProbability number, GenderProbability add)
        {
            GenderProbability result = new GenderProbability();
            result.Man = number.Man + add.Man;
            result.Woman = number.Woman + add.Woman;
            return number;
        }
    }
}
