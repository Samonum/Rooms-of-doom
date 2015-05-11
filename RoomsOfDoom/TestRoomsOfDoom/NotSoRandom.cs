using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRoomsOfDoom
{
    class NotSoRandom : Random
    {
        int randomInt = -1;
        double randomDouble = -1;
        Random r;
        public NotSoRandom(int rInt)
        {
            r = new Random();
            randomInt = rInt;
        }
        public NotSoRandom(double rDouble)
        {
            r = new Random();
            randomDouble = rDouble;
        }

        public override int Next(int maxValue)
        {
            return randomInt == -1 ? r.Next(maxValue) : Math.Min(maxValue, randomInt);
        }

        public override double NextDouble()
        {
            return randomDouble == -1 ? r.NextDouble() : randomDouble;
        }
    }
}
