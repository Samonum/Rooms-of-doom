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

        public Dungeon(int difficulty, List<Node> nodes)
        {
            this.difficulty = difficulty;
            this.nodes = nodes;
        }

        public List<Node> ShortestPath(int from, int to)
        {
            List<Node> path = new List<Node>();

            return path;
        }

        public void Destroy(int node)
        {

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
