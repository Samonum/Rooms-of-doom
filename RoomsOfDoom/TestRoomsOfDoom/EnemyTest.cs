using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class EnemyTest : HittableTest
    {
        Enemy e;
        Random r;
        public EnemyTest() : base()
        {
        }

        [TestInitialize]
        public void Init()
        {
            r = new Random();
            MonsterCreator M = new MonsterCreator(r, 1);
            e = M.GeneratePack(2)[0];
        }

        public override IHittable getHittable()
        {
            return e;
        }

        [TestMethod]
        public void EnemySetterTest()
        {
            //Arrange
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
