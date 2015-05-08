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
        private List<Node> nodes;
        private Node endNode;

        public Dungeon(int difficulty, List<Node> nodes)
        {
            this.difficulty = difficulty;
            this.nodes = nodes;
            endNode = nodes[nodes.Count - 1];
        }

        public List<Node> ShortestPath(int from, int to)
        {
            List<Node> path = new List<Node>();
            // TODO: implement BFS
            return path;
        }

        public bool Destroy(Node rNode)
        {
            List<Node> pre = new List<Node>();
            Queue<Node> queue = new Queue<Node>();

            if (rNode.isBridge())
                return false;

            if (rNode == endNode)
                return false;

            pre.Add(endNode);
            queue.Enqueue(endNode);

            while (queue.Count > 0)
            {
                Node curNode = queue.Dequeue();

                foreach (KeyValuePair<Direction, Node> kvp in curNode.AdjacencyList)
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
                        foreach (KeyValuePair<Direction, Node> neighbour in n.AdjacencyList)
                        {
                            Direction direction = neighbour.Value.AdjacencyList.First(kvp => kvp.Value == rNode).Key;
                            neighbour.Value.AdjacencyList.Remove(direction);
                        }

                        Dictionary<int, int> a = new Dictionary<int, int>();
                    }
                }
            }

            foreach (Node n in toBeRemoved)
                nodes.Remove(n);

            return true;
        }

        public bool AddPack(int nodeIndex, Pack pack)
        {
            if (nodeIndex >= nodes.Count)
                return false;

            return nodes[nodeIndex].AddPack(pack);
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
