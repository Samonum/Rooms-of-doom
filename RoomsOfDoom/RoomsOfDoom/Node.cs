using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Node
    {
        protected Dictionary<Exit, Node> adjacencyList;
        protected List<Pack> packs;
        public int id;
        protected string stringName;
        protected int capMultiplier;

        public Node(int id)
        {
            this.id = id;
            adjacencyList = new Dictionary<Exit, Node>();
            packs = new List<Pack>();
            stringName = "N";
            capMultiplier = 1;
        }

        public Dictionary<Exit, Node> AdjacencyList
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

        public virtual bool RemovePack(Pack pack)
        {
            if (!packs.Contains(pack))
                return false;

            packs.Remove(pack);

            // TODO: Return false when pack is not present
            // TODO: Change to byte/int in bridge to allow item dop logic
            
            return true;
        }

        public List<Pack> PackList
        {
            get { return packs; }
        }

        public Pack GetNextPack
        {
            get 
            {
                if (packs.Count == 0)
                    return null;
                return packs[0]; 
            }
        }

        public int CapMultiplier
        {
            get { return capMultiplier; }
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

            foreach (KeyValuePair<Exit, Node> n in adjacencyList)
                s += n.Value.id + ",";

            s += ")-[Packs: ";

            s += packs.Count + ", Monsters: ";

            s += MonsterCount + "]";

            return s;
        }

        public List<Pack> Packs
        {
            get { return packs; }
        }
    }
}
