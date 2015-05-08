using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class DungeonCreatorTest
    {
        Random random = new Random();

        [TestMethod]
        public void NodeConstructorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int id = random.Next(int.MinValue, int.MaxValue);
                //TODO maxCapacity
                Node n = new Node(random, id, 15);

                Assert.IsNotNull(n);
                //Assert.IsTrue()
            }
        }

        [TestMethod]
        public void BFSTest()
        {
            DungeonCreator D = new DungeonCreator(random);

            for (int i = 0; i < 100; i++)
            {
                // TODO: use higher numbers
                // TODO: with high pack count to difficulty ration, this could infinitely loop
                //TODO FIX
                //Dungeon d = D.CreateDungeon(random.Next(0, 10), random.Next(0, 10));
            }
        }

        [TestMethod]
        public void NoPathTest()
        {
            DungeonCreator D = new DungeonCreator(random);

            for (int i = 0; i < 100; i++)
            {
                // TODO: use higher numbers
                // TODO FIX
                /*Dungeon d = D.CreateDungeon(random.Next(0, 100), random.Next(0, 100));
                
                Assert.IsNotNull(d);
                Assert.IsTrue(d.Size > 2);

                foreach (Node n in d.nodes)
                {
                    Assert.IsTrue(n.AdjacencyList.Count > 0);
                    Assert.IsTrue(n.AdjacencyList.Count <= DungeonCreator.maxNeighbours);
                }*/
                
            }
        }
    }
}
