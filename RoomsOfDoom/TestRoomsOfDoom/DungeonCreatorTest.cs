using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class DungeonCreatorTest
    {
        Random r = new Random();

        [TestMethod]
        public void NodeConstructorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int id = r.Next(int.MinValue, int.MaxValue);
                Node n = new Node(id);

                Assert.IsNotNull(n);
                //Assert.IsTrue()
            }
        }

        [TestMethod]
        public void BFSTest()
        {
            DungeonCreator D = new DungeonCreator(r);

            for (int i = 0; i < 100; i++)
            {
                // TODO: use higher numbers
                // TODO: with high pack count to difficulty ration, this could infinitely loop
                Dungeon d = D.CreateDungeon(r.Next(0, 10), r.Next(0, 10));
            }
        }

        [TestMethod]
        public void DungeonCreatorScaleTest()
        {
            DungeonCreator D = new DungeonCreator(r);
            for (int i = 0; i < 100; i++)
            {
                Dungeon d = D.CreateDungeon(r.Next(500, 1000), r.Next(0, 30));
            }

        }

        [TestMethod]
        public void NoPathTest()
        {
            DungeonCreator D = new DungeonCreator(r);

            for (int i = 0; i < 100; i++)
            {
                // TODO: use higher numbers
                Dungeon d = D.CreateDungeon(r.Next(0, 100), r.Next(0, 100));
                
                Assert.IsNotNull(d);
                Assert.IsTrue(d.Size > 2);

                foreach (Node n in d.nodes)
                {
                    Assert.IsTrue(n.AdjacencyList.Count > 0);
                    Assert.IsTrue(n.AdjacencyList.Count <= DungeonCreator.maxNeighbours);
                }
                
            }
        }
    }
}
