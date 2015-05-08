using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class ArenaTest
    {
        Random random;
        public ArenaTest()
        {
            random = new Random();
        }

        [TestMethod]
        public void EnemyAdditionTest()
        {
            MonsterCreator creator = new MonsterCreator(new NotSoRandom(25), 25);
            Pack p = creator.GeneratePack(9001);
            Node node = new Node(random, 1, 15);
            node.AddPack(p);
            for(int n = 0; n < 100; n++)
            {
                GameManager a = new GameManager(node, new Player(), new Random());
                for (int i = 0; i < p.Size; i++)
                    for (int j = 0; j < i; j++)
                        Assert.AreNotEqual(p[i].Location, p[j].Location, "Two enemies at the same place. The horror!");
            }
        }
    }
}
