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
        protected Random random;
        protected List<Pack> packs;
        public int id;
        protected string stringName;
        protected int multiplier;
        protected int maxCapacity;

        public Node(Random random, int id, int maxCapacity, bool isExit = false)
        {
            this.id = id;
            this.random = random;
            adjacencyList = new Dictionary<Exit, Node>();
            packs = new List<Pack>();
            stringName = "N";
            multiplier = 1;
            this.maxCapacity = maxCapacity * multiplier;
            IsExit = isExit;
        }

        public void Update()
        {
            List<Pack> removeList = new List<Pack>();

            foreach (Pack p in PackList)
            {
                if (p.Size == 0)
                {
                    removeList.Add(p);
                    continue;
                }

                if (random.NextDouble() > 0.5)
                    continue;

                List<Node> choices = new List<Node>();

                foreach (KeyValuePair<Exit, Node> kvp in AdjacencyList)
                    choices.Add(kvp.Value);

                if (choices.Count == 0)
                    continue;

                Node to = choices[random.Next(choices.Count)];

                if (to.MonsterCount + p.Size > maxCapacity * to.Multiplier)
                    continue;

                to.AddPack(p);
                removeList.Add(p);
            }

            foreach (Pack p in removeList)
                PackList.Remove(p);
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

        public bool IsExit
        {
            get;
            private set;
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

        //
        //
        // TODO: You know what to do
        // Add a get total encumbrance property
        //

        public Pack GetNextPack
        {
            get 
            {
                if (packs.Count == 0)
                    return null;
                return packs[0]; 
            }
        }

        public int Multiplier
        {
            get { return multiplier; }
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
    }
}
