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
        public Bridge(int id, int bridgeNr)
            :base(id)
        {
            this.bridgeNr = bridgeNr;
            stringName = "B";
            capMultiplier = bridgeNr;
        }

        public override bool isBridge()
        {
            return true;
        }
    }
}
