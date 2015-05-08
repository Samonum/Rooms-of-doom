using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Dungeon
    {
        private int difficulty;
        //TODO for testing purposes it is public
        public List<Node> nodes;
        private Node endNode;
        private Random random;
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

        public void Update()
        {
            foreach (Node n in nodes)
            {
                n.Update();
            }
        }

        public List<Node> ShortestPath(int from, int to)
        {
            List<Node> path = new List<Node>();
            // TODO: implement BFS
            return path;
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
                            neighbour.Value.AdjacencyList.Remove(direction);
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

        public String ToString()
        {
            String s = "";

            foreach (Node n in nodes)
                s += n.ToString() + "\n";

            return s;
        }
    }
}
