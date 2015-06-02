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
        int maxCapacity;
        List<Node> availibleNodes;

        public DungeonCreator(Random random)
        {
            this.random = random;
            // TODO: Theoretically we have room for 11 of them
            monsterCreator = new MonsterCreator(random, 6);
        }

        public Dungeon CreateDungeon(int difficulty, int packCount, int maxCapacity)
        {
            if (packCount > difficulty * 15)
                packCount = difficulty * 15;

            this.maxCapacity = maxCapacity;

            Dungeon dungeon = GenerateDungeon(difficulty);
            dungeon = SpreadPacks(dungeon, difficulty, packCount);
            dungeon.ShortestPath(dungeon.nodes[0], dungeon.nodes[dungeon.nodes.Count - 1]);
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
            int counter = 1;

            for (int i = 0; i < size; i++)
            {
                Node n;
                if (i == bridgeTarget)
                    n = new Bridge(random, i, maxCapacity, counter);
                else
                    n = new Node(random, i, maxCapacity, i == size - 1);

                nodes.Add(n);

                int neighbourAmount = 1 + random.Next(maxNeighbours - 1);

                for (int j = 0; j < neighbourAmount; j++)
                {
                    if (availibleNodes.Count == 0)
                        continue;
                    int neighbourIndex = random.Next(availibleNodes.Count);
                    Node newNeighbour = availibleNodes[neighbourIndex];

                    //n.AddGate(newNeighbour);
                    
                    if (!n.AdjacencyList.ContainsValue(newNeighbour))
                    {
                        Exit rn = (Exit)Math.Pow(2, random.Next(maxNeighbours));
                        while (n.AdjacencyList.ContainsKey(rn))
                            rn = (Exit)Math.Pow(2, random.Next(maxNeighbours));

                        n.AddGate((Exit)rn, newNeighbour);

                        switch (rn)
                        {
                            case Exit.Top:
                                rn = Exit.Bot;
                                break;
                            case Exit.Bot:
                                rn = Exit.Top;
                                break;
                            case Exit.Left:
                                rn = Exit.Right;
                                break;
                            case Exit.Right:
                                rn = Exit.Left;
                                break;
                        }

                        while (newNeighbour.AdjacencyList.ContainsKey(rn))
                            rn = (Exit)Math.Pow(2, random.Next(maxNeighbours));    

                        newNeighbour.AddGate(rn, n);

                        if (newNeighbour.AdjacencyList.Count >= maxNeighbours)
                            availibleNodes.Remove(newNeighbour);
                    }
                }
                if (bridgeTarget == i)
                {
                    /*
                    foreach(Node availibleNode in availibleNodes)
                    {
                        if (availibleNode.AdjacencyList.Count == 0)
                        {
                            int neighbourIndex = random.Next(availibleNodes.Count);
                            Node newNeighbour = availibleNodes[neighbourIndex];

                            while (!n.AddGate(newNeighbour))
                            {
                                neighbourIndex = random.Next(availibleNodes.Count);
                                newNeighbour = availibleNodes[neighbourIndex];
                                while (newNeighbour == availibleNode)
                                    newNeighbour = availibleNodes[neighbourIndex];
                            }
                        }
                    }*/

                    availibleNodes.Clear();
                    if (counter < difficulty)
                    {
                        bridgeTarget += split;
                        counter++;
                    }
                }
                if (n.AdjacencyList.Count < maxNeighbours)
                    availibleNodes.Add(n);
            }

            // 100 7 =  14 (+ 2) {0 14 28 42 56 70 84 98}

            Dungeon dungeon = new Dungeon(random, nodes, difficulty, maxCapacity);
            return dungeon;
        }
        
        private Dungeon SpreadPacks(Dungeon dungeon, int difficulty, int packCount)
        {
            for (int i = 0; i < packCount; i++)
            {
                Pack pack = monsterCreator.GeneratePack(difficulty);
                Node node = dungeon.nodes[1 + random.Next(dungeon.Size - 1)];

                // TODO: This might be an infinite loop if dungeon is full
                while (!node.AddPack(pack))
                    node = dungeon.nodes[1 + random.Next(dungeon.Size - 1)];
            }

            return dungeon;
        }
    }
}
