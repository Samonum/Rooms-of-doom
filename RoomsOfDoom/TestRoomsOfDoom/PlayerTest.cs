using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomsOfDoom;
using RoomsOfDoom.Items;
using System.Drawing;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class PlayerTest : HittableTest
    {
        Player p;
        Random r;
        public PlayerTest() : base()
        {
        }

        [TestInitialize]
        public void Init()
        {
            p = new Player();
            r = new Random();
        }

        public override IHittable getHittable()
        {
            return p;
        }

        [TestMethod]
        public void TestMovement()
        {
            //arrange
            Pack testPack = new Pack(0);
            p.Location = new Point(5, 5);
            //act and assert
            p.Move(Direction.Up, testPack);
            Assert.IsTrue(p.Location.X == 5 && p.Location.Y == 4);
            p.Move(Direction.Left, testPack);
            Assert.IsTrue(p.Location.X == 4 && p.Location.Y == 4);
            p.Move(Direction.Down, testPack);
            Assert.IsTrue(p.Location.X == 4 && p.Location.Y == 5);
            p.Move(Direction.Right, testPack);
            Assert.IsTrue(p.Location.X == 5 && p.Location.Y == 5);
        }

        [TestMethod]
        public void TestMovementObstructed()
        {
            //arrange
            Pack testPack = new Pack(0);
            p.Location = new Point(5,1);
            //act and assert
            p.Move(Direction.Up, testPack);
            Assert.IsTrue(p.Location.X == 5 && p.Location.Y == 1);
            p.Location = new Point(1, 5);
            p.Move(Direction.Left, testPack);
            Assert.IsTrue(p.Location.X == 1 && p.Location.Y == 5);
            p.Location = new Point(6, Arena.Height -2);
            p.Move(Direction.Down, testPack);
            Assert.IsTrue(p.Location.X == 6 && p.Location.Y == Arena.Height -2);
            p.Location = new Point(Arena.Width - 2, 5);
            p.Move(Direction.Right, testPack);
            Assert.IsTrue(p.Location.X == Arena.Width -2 && p.Location.Y == 5);
        }

        [TestMethod]
        public void TestMovementObstructedByEnemy()
        {
            //arrange
            MonsterCreator M = new MonsterCreator(r, 10);
            Pack testPack = M.GeneratePack(1);
            Arena a = new Arena(Exit.Right, testPack, p, Exit.Right, r);
            testPack[0].Location = new Point(5, 5);
            p.Location = new Point(5,4);
            //act and assert
            p.Move(Direction.Down, testPack);
            Assert.IsTrue(p.Location.X == 5 && p.Location.Y == 4);

        }

        [TestMethod]
        public void PlayerTestAllTheThings()
        {
            Player p = new Player();
            char x = p.Glyph;
            p.CurrentHP = 50;
            for(int i = 0; i < 100; i++)
            {
                p.CurrentHP++;
            }

            Assert.IsTrue(p.MaxHP >= p.CurrentHP);

        }

        [TestMethod]
        public void PotionTest()
        {
            int pots = p.GetPotCount;
            int count = 100 + r.Next(50);
            for (int i = 0; i < count; i++)
            {
                p.AddPotion();
                pots++;
            }
            Assert.AreEqual(pots, p.GetPotCount, "Pots don't add up well.");
        }

        [TestMethod]
        public void CrystalTest()
        {
            int crystals = p.GetCrystalCount;
            int count = 100 + r.Next(50);
            for (int i = 0; i < count; i++)
            {
                p.AddCrystal();
                crystals++;
            }
            Assert.AreEqual(crystals, p.GetCrystalCount, "Crystals don't add up well.");
        }

        [TestMethod]
        public void ScrollTest()
        {
            int scrolls = p.GetScrollCount;
            int count = 100 + r.Next(50);
            for (int i = 0; i < count; i++)
            {
                p.AddScroll();
                scrolls++;
            }
            Assert.AreEqual(scrolls, p.GetScrollCount, "Scrolls don't add up well.");
        }

        [TestMethod]
        public void CombatItemEffectTest()
        {
            Pack enemies = new Pack(4);
            int initHp = 10 * Player.strength;
            for (int i = 0; i < 4; i++)
                enemies.Add(new Enemy("Test", 't', initHp));
            p.Combat(enemies[0]);

            Assert.AreEqual(initHp - Player.strength, enemies[0].CurrentHP);
            for (int i = 1; i < 3; i++)
                Assert.AreEqual(100, enemies[i].CurrentHP);
            
            p.UseItem(new TimeCrystal(), null);

            p.Combat(enemies[1]);

            Assert.AreEqual(initHp - 2 * Player.strength, enemies[0].CurrentHP);
            for (int i = 1; i < 3; i++)
                Assert.AreEqual(initHp - Player.strength, enemies[i].CurrentHP);
            p.UseItem(new MagicScroll(new NotSoRandom(0.0)), null);

            p.Combat(enemies[2]);

            Assert.AreEqual(initHp - 4 * Player.strength, enemies[0].CurrentHP);
            for (int i = 1; i < 3; i++)
                Assert.AreEqual(initHp - 3 * Player.strength, enemies[i].CurrentHP);
        }

    }
}
