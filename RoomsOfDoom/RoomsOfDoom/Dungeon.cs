using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    class Dungeon
    {
        private int difficulty;
        public Dungeon(int difficulty)
        {
            this.difficulty = difficulty;
        }

        public List<Node> ShortestPath(int from, int to)
        {
            List<Node> path = new List<Node>();

            return path;
        }

        public void Destroy(int node)
        {

        }
    }
}
