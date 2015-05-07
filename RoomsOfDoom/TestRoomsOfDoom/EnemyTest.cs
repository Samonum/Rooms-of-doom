using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class EnemyTest : HittableTest
    {

        public EnemyTest() : base()
        {

        }

        public override IHittable getHittable()
        {
            return new Enemy("test",'T', 100);
        }

        [TestMethod]
        public void EnemySetterTest()
        {
            //Arrange
            Random r = new Random();
            MonsterCreator M = new MonsterCreator(r,20);
            Enemy e = M.CreateMonster(1);
            //act
            e.CurrentHP = 1;
            for (int i = 0; i < 100; i++)
            {
                e.CurrentHP++;
            }

            Assert.IsTrue(e.MaxHP >= e.CurrentHP);

            e.MaxHP = 100;
            char c = e.Glyph;
            e.Alive = false;

        }
    }
}
