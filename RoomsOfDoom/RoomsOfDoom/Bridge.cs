using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Bridge : Node
    {
        // TODO bridgeNr may be redundant
        int bridgeNr;
        public Bridge(Random random, int id, int maxCapacity, int bridgeNr)
            :base(random, id, maxCapacity)
        {
            this.bridgeNr = bridgeNr;
            stringName = "B";
            multiplier = bridgeNr;
            maxCapacity = multiplier * maxCapacity;
        }
    }
}
