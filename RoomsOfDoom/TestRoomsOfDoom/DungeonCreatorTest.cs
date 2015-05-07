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
                Dungeon d = D.GenerateDungeon(r.Next(int.MinValue, int.MaxValue));

            }
        }

        [TestMethod]
        public void NoPathTest()
        {
            DungeonCreator D = new DungeonCreator(r);

            for (int i = 0; i < 100; i++)
            {
                Dungeon d = D.GenerateDungeon(r.Next(int.MinValue, int.MaxValue));
                
                Assert.IsNotNull(d);
                Assert.IsTrue(d.nodes.Count > 2);

                foreach (Node n in d.nodes)
                {
                    Assert.IsTrue(n.AdjacencyList.Count > 0);
                    Assert.IsTrue(n.AdjacencyList.Count <= DungeonCreator.maxNeighbours);
                }
                
            }
        }
    }
}
