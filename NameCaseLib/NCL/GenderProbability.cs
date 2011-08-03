using System;
using System.Collections.Generic;
using System.Linq;
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

        public GenderProbability operator+(GenderProbability add)
        {
            this.Man += add.Man;
            this.Woman += add.Woman;
            return this;
        }
    }
}
