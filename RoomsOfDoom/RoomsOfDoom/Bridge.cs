using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Bridge : Node
    {

        public Bridge(int id)
            :base(id)
        {
        }

        public override String ToString()
        {
            String s = "B" + id + "(";

            foreach (Node n in adjacencyList)
                s += n.id + ",";

            s += ")";

            return s;
        }
    }
}
