using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;
using RoomsOfDoom.Items;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class ItemTest
    {
        Random random;
        public ItemTest()
        {
            random = new Random();
        }

        [TestMethod]
        public void TestGlyphs()
        {
            Potion p = new Potion();
            Assert.IsTrue(p.Glyph == '1');

            TimeCrystal t = new TimeCrystal();
            Assert.IsTrue(t.Glyph == '2');

            GameManager g = new RoomsOfDoom.GameManager(false);

            MagicScroll m = new MagicScroll(random, g);
            Assert.IsTrue(m.Glyph == '3');

            LevelKey k = new LevelKey(g);
            Assert.IsTrue(k.Glyph == '>');
        }

        public void TestDuration()
        {

        }
    }
}
