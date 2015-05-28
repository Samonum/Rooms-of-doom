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
            MonsterCreator M = new MonsterCreator(r, 20);
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

        [TestMethod]
        [Timeout(5000)]
        public void EnemyMovementTest()//test if monsters do not enter the spaces of other monsters in their pack
        {
            //arrange
            MonsterCreator M = new MonsterCreator(r, 20);
            Pack pack = new Pack(10);

            while(pack.Enemies.Count < pack.Enemies.Capacity)
            {
                Enemy x = M.CreateMonster(1);
                pack.Add(x);

            }

            Player p = new Player();
            Enemy y = pack[0];
            p.Location = new System.Drawing.Point(5,5);
            pack[1].Location = new System.Drawing.Point(5, 4);
            pack[2].Location = new System.Drawing.Point(4, 5);
            pack[3].Location = new System.Drawing.Point(6, 5);
            pack[4].Location = new System.Drawing.Point(5, 6);
            pack[5].Location = new System.Drawing.Point(4, 4);
            pack[6].Location = new System.Drawing.Point(3, 3);

            y.Location = new System.Drawing.Point(2,2);
            //act
            for(int i = 0; i < 100;i++)
            {
                y.AggressiveMove(p);
                //assert
                foreach (Enemy en in pack)
                {
                    if(en != y)
                        Assert.AreNotEqual(y.Location, en.Location);
                }
            }


        }

        [TestMethod]
        public void PlayerDamageTest()
        {
            //arrange
            MonsterCreator M = new MonsterCreator(r, 10);
            Player p = new Player();
            p.Location = new System.Drawing.Point(5, 5);
            Pack pack = M.GeneratePack(1);
            Enemy x = pack[0];
            x.Location = new System.Drawing.Point(4, 5);
            //act
            x.AggressiveMove(p);
            //assert
            Assert.IsTrue(p.CurrentHP < p.MaxHP);
        }
    }
}
