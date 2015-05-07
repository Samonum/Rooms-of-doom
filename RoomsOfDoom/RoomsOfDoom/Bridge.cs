using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Bridge : Node
    {
        int bridgeNr;
        public Bridge(int id, int bridgeNr)
            :base(id)
        {
            this.bridgeNr = bridgeNr;
        }

        public override bool isBridge()
        {
            return true;
        }

        public override String ToString()
        {
            String s = "B" + id + "(";

            foreach (KeyValuePair<Direction, Node> n in adjacencyList)
                s += n.Value.id + ",";

            s += ")";

            return s;
        }
    }
}
