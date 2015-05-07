using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class DungeonCreator
    {
        Random random;

        const int maxNeighbours = 4;

        List<Node> availibleNodes;

        public DungeonCreator(Random random)
        {
            this.random = random;
        }

        public Dungeon GenerateDungeon(int size, int difficulty)
        {
            List<Node> nodes = new List<Node>();
            availibleNodes = new List<Node>();
            int split = size / (difficulty + 1);

            int counter = 0;

            for (int i = 0; i < size; i++)
            {
                Node n;
                if (counter == split)
                    n = new Bridge(i);
                else
                    n = new Node(i);

                nodes.Add(n);
                int neighbourAmount = 1 + random.Next(maxNeighbours - 1);
                for (int j = 0; j < neighbourAmount; j++)
                {
                    if (availibleNodes.Count == 0)
                        continue;
                    int neighbourIndex = random.Next(availibleNodes.Count);
                    Node newNeighbour = availibleNodes[neighbourIndex];

                    if (!n.AdjacencyList.Contains(newNeighbour))
                    {
                        n.AdjacencyList.Add(newNeighbour);
                        newNeighbour.AdjacencyList.Add(n);
                        if (newNeighbour.AdjacencyList.Count >= maxNeighbours)
                            availibleNodes.Remove(newNeighbour);
                    }
                }
                if (counter == split)
                {
                    availibleNodes.Clear();
                    counter = 0;
                }
                
                counter++;
                
                availibleNodes.Add(n);
                


                /*   1   
                 *   2  
                 *   3
                 *   4 
                 * 
                 * 
                 */
            }

            // 100 7 =  14 (+ 2) {0 14 28 42 56 70 84 98}

            Dungeon dungeon = new Dungeon(difficulty, nodes);
            return dungeon;
        }

        private Node CreateNode(bool isBridge, int id)
        {
            Node n;
            if (isBridge)
                n = new Bridge(id);
            else
                n = new Node(id);

            int neighbourAmount = random.Next(maxNeighbours);
            for (int j = 0; j < neighbourAmount; j++)
            {
                if (availibleNodes.Count == 0)
                    continue;
                int neighbourIndex = random.Next(availibleNodes.Count);
                Node newNeighbour = availibleNodes[neighbourIndex];

                if (!n.AdjacencyList.Contains(newNeighbour))
                {
                    n.AdjacencyList.Add(newNeighbour);
                    newNeighbour.AdjacencyList.Add(n);
                    if (newNeighbour.AdjacencyList.Count >= maxNeighbours)
                        availibleNodes.Remove(newNeighbour);
                }
            }
            if (isBridge)
                availibleNodes.Clear();
            availibleNodes.Add(n);
            return n;
        }

    }
}
