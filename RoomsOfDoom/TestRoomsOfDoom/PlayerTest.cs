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
            Node n = new Node(1);
            n.AddPack(testPack);
            Arena a = new Arena(n, p, Exit.Right, r);
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
            
            p.CurrentHP -= (int)(1.5 * Potion.healPower);
            p.UseItem(new Potion(), null);
            Assert.AreEqual(p.CurrentHP, p.MaxHP - (int)(.5 * Potion.healPower), "Simple healing");
            p.UpdateItems();
            Assert.AreEqual(p.CurrentHP, p.MaxHP - (int)(.5 * Potion.healPower), "Nothing happens");
            p.UseItem(new Potion(), null);
            Assert.AreEqual(p.CurrentHP, p.MaxHP, "No overcharging HP");

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
            p.inventory = new byte[] { 2, 2, 2 };

            Assert.AreEqual(initHp - Player.strength, enemies[0].CurrentHP, "base");
            for (int i = 1; i < 3; i++)
                Assert.AreEqual(100, enemies[i].CurrentHP, "base");

            TimeCrystal crystal = new TimeCrystal();
            crystal.Duration = 1;
            p.UseItem(crystal, null);

            p.Combat(enemies[1]);
            Assert.AreEqual(initHp - 2 * Player.strength, enemies[0].CurrentHP, "Crystal");
            for (int i = 1; i < 3; i++)
                Assert.AreEqual(initHp - Player.strength, enemies[i].CurrentHP, "Crystal");
            
            MagicScroll scroll = new MagicScroll(new NotSoRandom(0.0));
            scroll.Duration = 2;
            p.UseItem(scroll, null);

            p.Combat(enemies[2]);
            Assert.AreEqual(initHp - 4 * Player.strength, enemies[0].CurrentHP, "Scroll and Crystal");
            for (int i = 1; i < 3; i++)
                Assert.AreEqual(initHp - 3 * Player.strength, enemies[i].CurrentHP, "Scroll and Crystal");

            p.UpdateItems();
            p.Combat(enemies[3]);
            Assert.AreEqual(initHp - 6 * Player.strength, enemies[0].CurrentHP, "Scroll and Crystal last time");
            for (int i = 1; i < 3; i++)
                Assert.AreEqual(initHp - 5 * Player.strength, enemies[i].CurrentHP, "Scroll and Crystal last time");

            p.UpdateItems();
            p.Combat(enemies[0]);
            Assert.AreEqual(initHp - 8 * Player.strength, enemies[0].CurrentHP, "Scroll and no more Crystal");
            for (int i = 1; i < 3; i++)
                Assert.AreEqual(initHp - 5 * Player.strength, enemies[i].CurrentHP, "Scroll and no more Crystal");
            
            MagicScroll scroll2 = new MagicScroll(new NotSoRandom(0.0));
            scroll2.Duration = 0;
            p.UseItem(scroll2, null);

            p.Combat(enemies[1]);
            Assert.AreEqual(initHp - 8 * Player.strength, enemies[0].CurrentHP, "Back to Normal");
            Assert.AreEqual(initHp - 9 * Player.strength, enemies[1].CurrentHP, "Back to Normal");
            for (int i = 2; i < 3; i++)
                Assert.AreEqual(initHp - 5 * Player.strength, enemies[i].CurrentHP, "Back to Normal");

            p.UpdateItems();
            p.Combat(enemies[0]);
            Assert.AreEqual(initHp - 9 * Player.strength, enemies[0].CurrentHP, "Back to Normal");
            Assert.AreEqual(initHp - 9 * Player.strength, enemies[1].CurrentHP, "Back to Normal");
            for (int i = 2; i < 3; i++)
                Assert.AreEqual(initHp - 5 * Player.strength, enemies[i].CurrentHP, "Back to Normal");
        }

        [TestMethod]
        public void OutOfGoodStuffTest()
        {
            Enemy e = new Enemy("", 'l', 100);
            new Pack(0).Add(e);

            p.inventory = new byte[] { 0, 0, 0 };

            MagicScroll s = new MagicScroll(r);
            s.Duration = 0;
            Assert.IsFalse(p.UseItem(s, null), "Not Using");

            p.Combat(e);
            Assert.AreEqual(e.CurrentHP, e.MaxHP - Player.strength, "No multiplier when not used");

            p.UpdateItems();
            p.Combat(e);
            Assert.AreEqual(e.CurrentHP, e.MaxHP - 2 * Player.strength, "No neg multiplier when not used");
        }

    }
}
