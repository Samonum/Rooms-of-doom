using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomsOfDoom
{
    class DebugableRandom : Random
    {
        public DebugableRandom(int seed) : base(seed)
        {
            initialSeed = seed;
        }
        public int initialSeed;
        public int callCount = 0;

        public override int Next()
        {
            callCount++;
            return base.Next();
        }

        public override int Next(int i)
        {
            callCount++;
            return base.Next(i);
        }

        public override int Next(int i, int j)
        {
            callCount++;
            return base.Next(i, j);
        }

        public override double NextDouble()
        {
            callCount++;
            return base.NextDouble();
        }
    }
}
