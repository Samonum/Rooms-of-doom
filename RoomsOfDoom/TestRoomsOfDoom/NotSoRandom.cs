using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRoomsOfDoom
{
    class NotSoRandom : Random
    {
        int randomValue;
        public NotSoRandom(int value)
        {

        }

        public override int Next(int maxValue)
        {
            return Math.Max(maxValue, randomValue);
        }
    }
}
