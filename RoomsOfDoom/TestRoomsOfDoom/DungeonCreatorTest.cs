using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class DungeonCreatorTest
    {
        Random random;

        public DungeonCreatorTest()
        {
            random = new Random();
        }

        [TestMethod]
        public void NodeConstructorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int id = random.Next(int.MinValue, int.MaxValue);

                Node n = new Node(random, id, 15);

                Assert.IsNotNull(n);

            }
        }

        [TestMethod]
        public void VeryDifficultDungeonTest()
        {
            DungeonCreator D = new DungeonCreator(random);
            Dungeon dungeon = D.CreateDungeon(9001, 10, 15);
            Assert.IsTrue(dungeon.difficulty == dungeon.nodes.Count);
        }

        [TestMethod]
        public void GenerateCrampedDungeon()
        {
            for (int i = 0; i < 20; i++)
            {
                DungeonCreator D = new DungeonCreator(random);
                int maxCapacity = 15;
                Dungeon dungeon = D.CreateDungeon(1, 100, maxCapacity);
                foreach (Node n in dungeon.nodes)
                    Assert.IsTrue(n.MonsterCount <= maxCapacity, "Node" + n.id + " has " + n.MonsterCount);
            }
        }

        [TestMethod]
        public void NoPathTest()
        {
            DungeonCreator D = new DungeonCreator(random);

            for (int i = 0; i < 100; i++)
            {
                Dungeon d = D.CreateDungeon(1, 0, 15);
                
                Assert.IsNotNull(d);
                Assert.IsTrue(d.Size > 2);

                foreach (Node n in d.nodes)
                {
                    if (!(n.AdjacencyList.Count > 0))
                    {
                        int a = 0;
                        a = d.nodes.Count;
                    }
                    Assert.IsTrue(n.AdjacencyList.Count > 0, "No" + i + " "+ n.id);
                    Assert.IsTrue(n.AdjacencyList.Count <= DungeonCreator.maxNeighbours);
                }
                
            }
        }

        [TestMethod]
        public void BridgeChecker()
        {
            DungeonCreator D = new DungeonCreator(random);
            Dungeon d = D.CreateDungeon(1, 10, 15);
            Assert.IsFalse(d.nodes[0].isBridge());
            Assert.IsTrue(d.nodes[3].isBridge());
        }

        [TestMethod]
        public void AddRemoveGateTest()
        {
            Node a = new Node(random, 0, 15);
            Node b = new Node(random, 1, 15);
            Node c = new Node(random, 2, 15);

            Assert.IsTrue(a.AddGate(Exit.Right, b));
            Assert.IsFalse(a.AddGate(Exit.Left, b));
            Assert.IsFalse(a.AddGate(Exit.Right, c));
            Assert.IsTrue(a.AddGate(Exit.Left, c));

            Assert.IsFalse(a.RemoveGate(Exit.Bot));
            Assert.IsTrue(a.RemoveGate(Exit.Left));
            Assert.IsFalse(a.RemoveGate(Exit.Left));
        }
    }
}
