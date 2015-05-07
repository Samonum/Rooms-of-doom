using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Node
    {
        protected List<Node> adjacencyList;
        public int id;
        List<Pack> pack;

        public Node(int id)
        {
            this.id = id;
            adjacencyList = new List<Node>();
        }

        public List<Node> AdjacencyList
        {
            get { return adjacencyList; }
            set { adjacencyList = value; }
        }

        public virtual String ToString()
        {
            String s = "N" + id + "(";

            foreach (Node n in adjacencyList)
                s += n.id + ",";

            s += ")";

            return s;
        }
    }
}
