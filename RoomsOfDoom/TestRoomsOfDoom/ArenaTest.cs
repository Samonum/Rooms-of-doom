using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class ArenaTest
    {
        public ArenaTest()
        {

        }

        [TestMethod]
        public void EnemyAdditionTest()
        {
            MonsterCreator creator = new MonsterCreator(new NotSoRandom(25), 25);
            Pack p = creator.GeneratePack(9001);
            for(int n = 0; n < 100; n++)
            {
                Arena a = new Arena(Exit.Top, p, new Player(), Exit.Top, new Random());
                for (int i = 0; i < p.Size; i++)
                    for (int j = 0; j < i; j++)
                        Assert.AreNotEqual(p[i].Location, p[j].Location, "Two enemies at the same place. The horror!");
            }
        }
    }
}
