using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    public abstract class HittableTest
    {

        public HittableTest()
        {
        }

        public abstract IHittable getHittable();

        [TestMethod]
        public void Hitting()
        {
            Random r = new Random();
            IHittable testSubject = getHittable();
            int hp = testSubject.CurrentHP;
            while (hp > 1)
            {
                int hit = r.Next(hp - 1) + 1;
                testSubject.Hit(hit);
                Assert.IsTrue(testSubject.CurrentHP < hp, "hp >= Current HP; Old: " + hp + ", New: " + testSubject.CurrentHP + " Damage: " + hit);
                Assert.IsTrue(testSubject.CurrentHP > 0, "hp < 0; Old: " + hp + ", New: " + testSubject.CurrentHP + " Damage: " + hit);
                Assert.IsTrue(testSubject.Alive, "Death; Old: " + hp + ", New: " + testSubject.CurrentHP + " Damage: " + hit);
                hp = testSubject.CurrentHP;
            }
        }

        //People die when they're killed
        [TestMethod]
        public void DeathAtZero()
        {
            IHittable testSubject = getHittable();
            testSubject.Hit(testSubject.CurrentHP);
            Assert.AreEqual(testSubject.CurrentHP, 0, "HP: " + testSubject.CurrentHP);
            Assert.IsFalse(testSubject.Alive, "Survived");
        }

        //People really die when they're killed
        [TestMethod]
        public void OverKill()
        {
            IHittable testSubject = getHittable();
            testSubject.Hit(int.MaxValue);
            Assert.IsTrue(testSubject.CurrentHP <= 0, "HP: " + testSubject.CurrentHP);
            Assert.IsFalse(testSubject.Alive, "Survived");
        }
    }
}
