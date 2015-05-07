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
        public List<Node> nodes;
        public Node startNode, endNode;

        public Dungeon(int difficulty, List<Node> nodes)
        {
            this.difficulty = difficulty;
            this.nodes = nodes;
            startNode = nodes[0];
            endNode = nodes[nodes.Count - 1];
        }

        public List<Node> ShortestPath(int from, int to)
        {
            List<Node> path = new List<Node>();

            return path;
        }

        public void Destroy(Node node)
        {
            
        }

        public bool IsValidDungeon(Node rNode)
        {
            List<Node> pre = new List<Node>();
            Queue<Node> queue = new Queue<Node>();

            if (rNode.isBridge())
                return false;

            if (rNode == startNode)
                return false;

            if (rNode == endNode)
                return false;

            pre.Add(rNode);
            queue.Enqueue(rNode);

            bool foundIt = false;

            while (queue.Count > 0)
            {
                Node curNode = queue.Dequeue();

                foreach(Node nextNode in curNode.AdjacencyList)
                {
                    if (!pre.Contains(nextNode))
                    {
                        if (nextNode == rNode)
                            continue;

                        if (nextNode == endNode)
                            foundIt = true;

                        queue.Enqueue(nextNode);
                        pre.Add(nextNode);
                    }
                }
            }

            if (foundIt)
            {
                List<Node> toBeRemoved = new List<Node>();
                
                foreach(Node n in nodes)
                {
                    if (!pre.Contains(n))
                    {
                        toBeRemoved.Add(n);

                        if (n == rNode)
                        {
                            foreach (Node neighbour in n.AdjacencyList)
                                neighbour.AdjacencyList.Remove(rNode);
                        }
                    }
                }

                foreach (Node n in toBeRemoved)
                    nodes.Remove(n);

                return true;
            }

            return false;
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
