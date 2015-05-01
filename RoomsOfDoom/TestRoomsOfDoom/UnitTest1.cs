using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class PlayerTest
    {
        Player p;
        Random r;
        public PlayerTest()
        {
            p = new Player(); r = new Random();

        }

        [TestMethod]
        public void Hitting()
        {
            int hp = p.CurrentHP;
            while (hp > 1)
            {
                int hit = r.Next(hp - 1) + 1;
                p.Hit(hit);
                Assert.IsTrue(p.CurrentHP < hp, "hp >= Current HP; Old: " + hp + ", New: " + p.CurrentHP + " Damage: " + hit);
                Assert.IsTrue(p.CurrentHP > 0, "hp < 0; Old: " + hp + ", New: " + p.CurrentHP + " Damage: " + hit);
                Assert.IsTrue(p.Alive, "Death; Old: " + hp + ", New: " + p.CurrentHP + " Damage: " + hit);
                hp = p.CurrentHP;
            }
        }

        [TestMethod]
        public void DeathAtZero()
        {
            p.Hit(p.CurrentHP);
            Assert.AreEqual(p.CurrentHP, 0, "HP: " + p.CurrentHP);
            Assert.IsFalse(p.Alive, "Survived");
        }

        [TestMethod]
        public void OverKill()
        {
            p.Hit(int.MaxValue);
            Assert.IsTrue(p.CurrentHP <= 0, "HP: " + p.CurrentHP);
            Assert.IsFalse(p.Alive, "Survived");

        }
    }
}
