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
        public void LootConsistencyTest()
        {
            for (int i = 0; i < 50; i++)
            {
                Player p = new Player(50);
                Random r = new Random();
                int seed = r.Next();
                
                r = new Random(seed);
                DungeonCreator dc = new DungeonCreator(r);
                Dungeon d = dc.CreateDungeon(1, 3, 20);
                int rand = r.Next();

                ItemGenerator.Init(r, d, p);
                Loot l = ItemGenerator.GetItem(1);
                Loot lb = ItemGenerator.GetItem(1);
                Loot lc = ItemGenerator.GetItem(1);

                int randb = r.Next();

                r = new Random(seed);
                dc = new DungeonCreator(r);
                d = dc.CreateDungeon(1, 3, 20);
                int rand2 = r.Next();
                Assert.AreEqual(rand, rand2, "DC Random unequal.");

                ItemGenerator.Init(r, d, p);
                Loot l2 = ItemGenerator.GetItem(1);
                Loot lb2 = ItemGenerator.GetItem(1);
                Loot lc2 = ItemGenerator.GetItem(1);

                int randb2 = r.Next();
                Assert.AreEqual(randb, randb2, "Random unequal.");

                if (l != null && l2 != null)
                    Assert.AreEqual(l.ID, l2.ID, "Item 1 unequal");
                if (lb != null && lb2 != null)
                    Assert.AreEqual(lb.ID, lb2.ID, "Item 2 unequal");
                if (lc != null && lc2 != null)
                    Assert.AreEqual(lc.ID, lc2.ID, "Item 3 unequal");
            }
        }

        [TestMethod]
        public void MagicScrollUseTest()
        {
            GameManager g = new GameManager(false);
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
