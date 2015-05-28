using RoomsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public partial class Node
    {
        protected Dictionary<Exit, Node> adjacencyList;

        //protected Dictionary<Exit, Gate> gateList;
        protected Random random;
        protected List<Pack> packs;
        public int id;
        protected string stringName;
        protected int multiplier;
        protected int maxCapacity;
        public bool locked;

        public Node(Random random, int id, int maxCapacity, bool isExit = false)
        {
            this.id = id;
            this.random = random;
            adjacencyList = new Dictionary<Exit, Node>();
            //gateList = new Dictionary<Exit, Gate>();
            packs = new List<Pack>();
            stringName = "N";
            multiplier = 1;
            this.maxCapacity = maxCapacity * multiplier;
            IsExit = isExit;
            Player = null;
            locked = false;
            InitSizes();
        }

        public void MacroUpdate()
        {
            List<Pack> removeList = new List<Pack>();

            foreach (Pack p in PackList)
            {
                if (p.Size == 0)
                {
                    removeList.Add(p);
                    continue;
                }

                Node to;

                if (p.Target != null)
                {
                    List<Node> path = ShortestPath(p.Target);

                    // Target node does not exist (anymore)
                    if (path == null)
                    {
                        p.GiveOrder(null);
                        continue;
                    }

                    if (path.Count == 0)
                        continue;

                    to = path[0];
                }

                else
                {
                    if (random.NextDouble() > 0.5)
                        continue;

                    List<Node> choices = new List<Node>();

                    foreach (KeyValuePair<Exit, Node> kvp in AdjacencyList)
                        choices.Add(kvp.Value);

                    if (choices.Count == 0)
                        continue;

                    to = choices[random.Next(choices.Count)];
                }

                if (to.AddPack(p))
                    removeList.Add(p);
            }

            foreach (Pack p in removeList)
                PackList.Remove(p);
        }

        public List<Node> ShortestPath(Node to)
        {
            List<Node> path = new List<Node>();

            if (to == this)
                return path;

            Dictionary<Node, Node> pre = new Dictionary<Node, Node>();
            Queue<Node> queue = new Queue<Node>();

            pre.Add(to, to);
            queue.Enqueue(to);

            while (queue.Count > 0)
            {
                Node curNode = queue.Dequeue();

                foreach (KeyValuePair<Exit, Node> kvp in curNode.AdjacencyList)
                {
                    Node nextNode = kvp.Value;
                    if (!pre.ContainsKey(nextNode))
                    {
                        queue.Enqueue(nextNode);
                        pre.Add(nextNode, curNode);
                        if (nextNode == this)
                        {
                            Node n = nextNode;

                            // Noticed infinite loop due to 
                            while (n != pre[n])
                            {
                                n = pre[n];
                                path.Add(n);
                            }

                            //path.Add(n);
                            return path;
                        }
                    }
                }
            }

            return null;
        }

        public Dictionary<Exit, Node> AdjacencyList
        {
            get { return adjacencyList; }
        }

        /*
        public Dictionary<Exit, Gate> GateList
        {
            get { return gateList; }
        }
        */

        public bool AddGate(Exit exit, Node node)
        {
            if (AdjacencyList.ContainsKey(exit))
                return false;

            if (AdjacencyList.ContainsValue(node))
                return false;

            adjacencyList.Add(exit, node);
            exits |= exit;

            /*
            Gate gate = new Gate(1, 1, node);

            GateList.Add(exit, gate);
            */
            return true;
        }

        public bool RemoveGate(Exit exit)
        {
            if (!AdjacencyList.ContainsKey(exit))
                return false;

            adjacencyList.Remove(exit);
            exits &= ~exit;

            return true;
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

        public bool AddPack(Pack pack)
        {
            // Fixed maxCapacity issue
            if (GetUsedCapacity() + pack.Size > maxCapacity)
                return false;

            packs.Add(pack);

            return true;
        }

        public bool RemovePack(Pack pack)
        {
            if (!packs.Contains(pack))
                return false;

            packs.Remove(pack);
            
            return true;
        }

        public List<Pack> PackList
        {
            get { return packs; }
        }


        public int GetUsedCapacity()
        {
            int total = 0;

            foreach (Pack p in packs)
                total += p.Size;

            return total;
        }

        public int Multiplier
        {
            get { return multiplier; }
        }

        public Player Player
        {
            get;
            set;
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
            string s = "";

            if (Player != null)
                s += ">";

            if (locked)
                s += "!";

            s += stringName + id + "(";

            foreach (KeyValuePair<Exit, Node> kvp in AdjacencyList)
                s += kvp.Value.id + ",";

            s += ")[";

            foreach (Pack p in PackList)
                s += p.ToString();

            s += "]";

            return s;
        }
    }
}
