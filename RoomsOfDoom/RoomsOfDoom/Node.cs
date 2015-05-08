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
        protected List<Pack> packs;
        public int id;
        protected string stringName;

        public Node(int id)
        {
            this.id = id;
            adjacencyList = new Dictionary<Direction, Node>();
            packs = new List<Pack>();
            stringName = "N";
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

        public virtual bool AddPack(Pack pack)
        {
            packs.Add(pack);

            // TODO: Return false when node is full
            // TODO: Override in bridge to allow higher capacity
            return true;
        }

        public int MonsterCount
        {
            get 
            {
                int total = 0;
                foreach (Pack p in packs)
                    total += p.Size;
                return total;
            }
        }

        public String ToString()
        {
            String s = stringName + id + "(";

            foreach (KeyValuePair<Direction, Node> n in adjacencyList)
                s += n.Value.id + ",";

            s += ")-[Packs: ";

            s += packs.Count + ", Monsters: ";

            s += MonsterCount + "]";

            return s;
        }
    }
}
