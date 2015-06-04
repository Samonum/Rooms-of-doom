using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;
using System.Collections.Generic;
using RoomsOfDoom.Items;
using System.Drawing;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class NodeTest
    {
        Random random;
        public NodeTest()
        {
            random = new Random();
        }

        [TestMethod]
        public void RemovePackTest()
        {
            Node node = new Node(random, 0, 20);
            Pack a = new Pack(5);
            Pack b = new Pack(5);
            node.AddPack(a);

            Assert.IsFalse(node.RemovePack(b));
            Assert.IsTrue(node.RemovePack(a));
            Assert.IsFalse(node.RemovePack(a));
        }

        [TestMethod]
        public void UpdateTest()
        {
            Node node = new Node(new NotSoRandom(0.0), 0, 20);
            Pack a = new Pack(0);
            node.AddPack(a);
            node.MacroUpdate();
            Assert.IsTrue(node.PackList.Count == 0);

            MonsterCreator mc = new MonsterCreator(random, 6);
            Pack b = mc.GeneratePack(1);
            node.AddPack(b);
            node.MacroUpdate();
            Assert.IsTrue(node.PackList.Count == 1);
        }

        [TestMethod]
        public void UnlockNodeTest()
        {
            List<Node> nodes = new List<Node>();
            Bridge b = new Bridge(random, 0, 10, 1);
            Dungeon d = new Dungeon(random, nodes, 1, 10);
            Player player = new Player(101);
            ItemGenerator.Init(random, d, new Player());
            Pack p = new Pack(1);
            p.Add(new Enemy("A", 'a', 1));
            b.AddPack(p);
            b.MicroUpdates();
            Assert.IsTrue(b.locked);
            player.OP = true;
            player.Combat(p.Enemies[0]);
            b.MicroUpdates();
            Assert.IsFalse(b.locked);
            b.locked = true;
            b.MicroUpdates();
            Assert.IsFalse(b.locked);
        }

        [TestMethod]
        public void FleeingEnemyTest()
        {
            List<Node> nodes = new List<Node>();
            Node a = new Node(random, 0, 10);
            Node b = new Node(random, 1, 10);
            b.AddGate(Exit.Left, a);
            b.leftLocation = Node.minDoorSize;
            Dungeon d = new Dungeon(random, nodes, 1, 10);
            Player player = new Player();
            b.Player = player;
            player.Location = new Point(5, b.leftLocation);
            Pack p = new Pack(1);
            p.Add(new Enemy("A", 'a', 10, 5, 5));
            b.AddPack(p);
            p.Enemies[0].Location = new Point(6, b.leftLocation);
            p.Enemies[0].Hit(9);
            b.MicroUpdates();
            Assert.AreEqual(p.Enemies[0].Location, new Point(6, b.leftLocation));
            Assert.AreEqual(player.CurrentHP, player.MaxHP);
        }
    }
}
