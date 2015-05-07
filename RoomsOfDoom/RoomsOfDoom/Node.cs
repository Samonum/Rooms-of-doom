using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Node
    {
        protected Dictionary<Direction, Node> adjacencyList;
        public int id;
        List<Pack> pack;

        public Node(int id)
        {
            this.id = id;
            adjacencyList = new Dictionary<Direction, Node>();
        }

        public Dictionary<Direction, Node> AdjacencyList
        {
            get { return adjacencyList; }
            set { adjacencyList = value; }
        }

        public virtual bool isBridge()
        {
            return false;
        }

        public virtual String ToString()
        {
            String s = "N" + id + "(";

            foreach (KeyValuePair<Direction, Node> n in adjacencyList)
                s += n.Value.id + ",";

            s += ")";

            return s;
        }
    }
}
