using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;
using RoomsOfDoom.Items;
using System.Collections.Generic;

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
        public void GlyphsTest()
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

        [TestMethod]
        public void MagicScrollUseTest()
        {
            GameManager g = new RoomsOfDoom.GameManager(false);
            MagicScroll m = new MagicScroll(new NotSoRandom(1.0), g);
            DungeonCreator d = new DungeonCreator(random);
            Dungeon dungeon = d.CreateDungeon(1, 10, 15);
            g.InitRoom(dungeon.nodes[0]);
            int size = dungeon.nodes.Count;
            Player p = new Player();
            m.Use(p, dungeon);
            Assert.IsTrue(size > dungeon.nodes.Count);

            List<Node> nodes = new List<Node>();
            nodes.Add(new Node(random, 0, 15, true));
            dungeon = new Dungeon(random, nodes, 1, 15);
            g.InitRoom(dungeon.nodes[0]);
            m.Use(p, dungeon);
            Assert.IsTrue(dungeon.nodes.Count == 1);
        }
    }
}
