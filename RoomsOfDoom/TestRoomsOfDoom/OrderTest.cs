using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class OrderTest
    {
        Random random;
        public OrderTest()
        {
            random = new Random();
        }

        [TestMethod]
        public void DefendOrderTest()
        {
            List<Node> nodes = new List<Node>();
            Node a = new Node(random, 0, 15);
            Bridge b = new Bridge(random, 1, 15, 1);
            Node c = new Node(random, 2, 15);
            Bridge d = new Bridge(random, 2, 15, 2);
            Node e = new Node(random, 2, 15);

            a.AddGate(Exit.Right, b);
            b.AddGate(Exit.Left, a);
            b.AddGate(Exit.Right, c);
            c.AddGate(Exit.Left, b);
            c.AddGate(Exit.Right, d);
            d.AddGate(Exit.Left, c);
            d.AddGate(Exit.Right, e);
            e.AddGate(Exit.Left, d);

            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(c);
            nodes.Add(d);
            nodes.Add(e);
            MonsterCreator mc = new MonsterCreator(random, 1);
            Pack p = mc.GeneratePack(1);
            c.AddPack(p);
            Dungeon dungeon = new Dungeon(random, nodes, 1, 15);
            Assert.IsTrue(dungeon.DefendOrder());
            dungeon.MacroUpdate();
            Assert.IsNotNull(b.CurrentPack);
            b.locked = false;
            p.GiveOrder(new Order(d));
            p = mc.GeneratePack(1);
            c.AddPack(p);
            p = mc.GeneratePack(1);
            a.AddPack(p);
            Assert.IsTrue(dungeon.DefendOrder());

            for (int i = 0; i < 10; i++)
            {
                dungeon.MacroUpdate();
            }

            Assert.IsTrue(d.PackList.Count == 2);
        }

        [TestMethod]
        public void NoBridgeDefendOrderTest()
        {
            List<Node> nodes = new List<Node>();

            Node a = new Node(random, 1, 10);
            Node b = new Node(random, 2, 10);
            nodes.Add(a);
            nodes.Add(b);

            MonsterCreator mc = new MonsterCreator(random, 1);
            Pack p = mc.GeneratePack(1);
            a.AddPack(p);

            Dungeon d = new Dungeon(random, nodes, 1, 10);
            Assert.IsFalse(d.DefendOrder());
        }
    }
}
