using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Dungeon
    {
        public int difficulty;
        //TODO for testing purposes it is public
        public List<Node> nodes;
        public Node endNode;
        public Random random;
        private int maxCapacity;

        public Dungeon(Random random, List<Node> nodes, int difficulty, int maxCapacity)
        {
            this.difficulty = difficulty;
            this.nodes = nodes;
            this.random = random;
            this.maxCapacity = maxCapacity;

            if (nodes == null || nodes.Count == 0)
                endNode = null;
            else
                endNode = nodes[nodes.Count - 1];
        }

        public void MacroUpdate()
        {
            DefendOrder();
            foreach (Node n in nodes)
            {
                n.MacroUpdate();
            }
        }

        public void DefendOrder()
        {
            Bridge b = GetLastUnconqueredBridge();
            if (b == null)
                return;

            int deficit = b.bridgeNr - b.PackList.Count;
            GiveOrder(new Order(b), deficit);
        }

        public Bridge GetLastUnconqueredBridge()
        {
            int counter = 1;
            List<Bridge> bridges = new List<Bridge>();
            
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].isBridge())
                {
                    Bridge b = (Bridge)nodes[i];
                    if (b.bridgeNr == counter)
                    {
                        counter++;
                        if (b.locked)
                            return b;
                    }
                    else if (b.locked)
                        bridges.Add(b);
                }
            }

            foreach (Bridge b in bridges)
                if (b.bridgeNr == counter)
                    return b;

            return null;
        }

        public void GiveOrder(Order o, int count)
        {
            if (o == null)
                return;

            for (int i = 0; i < count; i++)
            {
                if (!SingleOrder(o))
                    return;
            }
        }

        private bool SingleOrder(Order o)
        {
            foreach (Node n in nodes)
            {
                if (o.Target == n)
                    continue;
                foreach (Pack p in n.PackList)
                {
                    if (p.GiveOrder(o))
                        return true;
                }
            }
            return false;
        }

        public List<Node> ShortestPath(Node from, Node to)
        {
            if (!nodes.Contains(from))
                return null;

            if (!nodes.Contains(to))
                return null;

            return from.ShortestPath(to);
        }

        public List<Node> Destroy(Node rNode)
        {
            List<Node> pre = new List<Node>();
            Queue<Node> queue = new Queue<Node>();

            // TODO: Pretty sure bridges can be destroyed
            /*
            if (rNode.isBridge())
                return false;
            */

            if (rNode == endNode)
                return null;

            pre.Add(endNode);
            queue.Enqueue(endNode);

            while (queue.Count > 0)
            {
                Node curNode = queue.Dequeue();

                foreach (KeyValuePair<Exit, Node> kvp in curNode.AdjacencyList)
                {
                    Node nextNode = kvp.Value;
                    if (!pre.Contains(nextNode))
                    {
                        if (nextNode == rNode)
                            continue;

                        queue.Enqueue(nextNode);
                        pre.Add(nextNode);
                    }
                }
            }

            List<Node> toBeRemoved = new List<Node>();

            foreach (Node n in nodes)
            {
                if (!pre.Contains(n))
                {
                    toBeRemoved.Add(n);

                    if (n == rNode)
                    {
                        foreach (KeyValuePair<Exit, Node> neighbour in n.AdjacencyList)
                        {
                            Exit direction = neighbour.Value.AdjacencyList.First(kvp => kvp.Value == rNode).Key;
                            neighbour.Value.RemoveGate(direction);
                        }

                        Dictionary<int, int> a = new Dictionary<int, int>();
                    }
                }
            }

            foreach (Node n in toBeRemoved)
                nodes.Remove(n);

            List<Node> validNeighbours = new List<Node>();

            foreach (KeyValuePair<Exit, Node> kvp in rNode.AdjacencyList)
            {
                Node n = kvp.Value;
                if (nodes.Contains(n))
                    validNeighbours.Add(n);
            }

            return validNeighbours;
        }
                    
        public int Size
        {
            get { return nodes.Count; }
        }

        public Node PlayerNode
        {
            get;
            set;
        }

        public String ToString()
        {
            string s = "";

            foreach (Node n in nodes)
                s += n.ToString() + "\n";
            return s;
        }
    }
}
