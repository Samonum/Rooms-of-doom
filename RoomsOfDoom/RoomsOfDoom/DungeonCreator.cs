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
        MonsterCreator monsterCreator;
        public const int maxNeighbours = 4;

        List<Node> availibleNodes;

        public DungeonCreator(Random random)
        {
            this.random = random;
            monsterCreator = new MonsterCreator(random, 6);
        }

        public Dungeon CreateDungeon(int difficulty, int packCount)
        {
            Dungeon dungeon = GenerateDungeon(difficulty);
            dungeon = SpreadPacks(dungeon, difficulty, packCount);
            return dungeon;
        }

        private Dungeon GenerateDungeon(int difficulty)
        {
            int size = (int)(difficulty * (3f + random.NextDouble()) + 4);

            // Added due to memoryoutofrangeexception
            if (size > 1000)
                size = 1000;

            int split;
            if (difficulty >= size)
            {
                difficulty = size;
                split = 1;
            }
            else
                split = size / (difficulty + 1);

            List<Node> nodes = new List<Node>();
            availibleNodes = new List<Node>();
            
            int bridgeTarget = split;
            int counter = 0;

            for (int i = 0; i < size; i++)
            {
                Node n;
                if (i == bridgeTarget)
                    n = new Bridge(i, counter);
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

                    if (!n.AdjacencyList.ContainsValue(newNeighbour))
                    {
                        int rn = random.Next(maxNeighbours);
                        while (n.AdjacencyList.ContainsKey((Exit) rn))
                            rn = random.Next(maxNeighbours);

                        n.AdjacencyList.Add((Exit)rn, newNeighbour);

                        rn = random.Next(maxNeighbours);
                        while (newNeighbour.AdjacencyList.ContainsKey((Exit)rn))
                            rn = random.Next(maxNeighbours);

                        newNeighbour.AdjacencyList.Add((Exit)rn, n);

                        if (newNeighbour.AdjacencyList.Count >= maxNeighbours)
                            availibleNodes.Remove(newNeighbour);
                    }
                }
                if (bridgeTarget == i)
                {
                    availibleNodes.Clear();
                    if (counter < difficulty - 1)
                    {
                        bridgeTarget += split;
                        counter++;
                    }
                }
                
                availibleNodes.Add(n);
            }

            // 100 7 =  14 (+ 2) {0 14 28 42 56 70 84 98}

            Dungeon dungeon = new Dungeon(difficulty, nodes, random);
            return dungeon;
        }
        
        private Dungeon SpreadPacks(Dungeon dungeon, int difficulty, int packCount)
        {
            for (int i = 0; i < packCount; i++)
            {
                Pack pack = monsterCreator.GeneratePack(difficulty);
                int randomNumber = 1 + random.Next(dungeon.Size - 1);
                
                // TODO: This might be an infinite loop if dungeon is full
                while (!dungeon.AddPack(randomNumber, pack))
                    randomNumber = 1 + random.Next(dungeon.Size - 1);
            }

            return dungeon;
        }
    }
}
