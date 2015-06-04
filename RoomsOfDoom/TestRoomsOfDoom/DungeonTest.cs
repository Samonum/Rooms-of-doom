using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class DungeonTest
    {
        public Random random;

        public DungeonTest()
        {
            random = new Random();
        }

        [TestMethod]
        public void DestroyLineTest()
        {
            List<Node> nodes = new List<Node>();
            Node a = new Node(random, 0, 15);
            Node b = new Node(random, 1, 15);
            Node c = new Node(random, 2, 15);
            Node d = new Node(random, 2, 15);
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

            Dungeon dungeon = new Dungeon(random, nodes, 1, 15);

            dungeon.Destroy(d);

            Assert.IsTrue(!dungeon.nodes.Contains(a));
            Assert.IsTrue(!dungeon.nodes.Contains(b));
            Assert.IsTrue(!dungeon.nodes.Contains(c));
            Assert.IsTrue(!dungeon.nodes.Contains(d));
            Assert.IsTrue(dungeon.nodes.Contains(e));
        }

        [TestMethod]
        public void DestroyEndTest()
        {
            DungeonCreator dc = new DungeonCreator(random);
            Dungeon dungeon = dc.CreateDungeon(1, 10, 15);
            List<Node> nodes = dungeon.Destroy(dungeon.nodes[dungeon.nodes.Count - 1]);
            Assert.IsNull(nodes);
        }

        [TestMethod]
        public void EmptyDungeonTest()
        {
            Dungeon dungeon = new Dungeon(random, null, 1, 15);
            Assert.IsNull(dungeon.endNode);

            dungeon = new Dungeon(random, new List<Node>(), 1, 15);
            Assert.IsNull(dungeon.endNode);
        }

        [TestMethod]
        public void DestroyEverythingTest()
        {
            for (int i = 0; i < 10; i++)
            {
                DungeonCreator dc = new DungeonCreator(random);
                Dungeon dungeon = dc.CreateDungeon(random.Next(100), random.Next(100), 15);
                Node lastNode = dungeon.nodes[dungeon.nodes.Count-1];
                
                while (dungeon.nodes.Count > 2)
                {
                    Node n = dungeon.nodes[random.Next(dungeon.nodes.Count - 1)];
                    List<Node> exits = dungeon.Destroy(n);
                    Assert.IsNotNull(exits);
                }

                Assert.IsTrue(dungeon.nodes.Contains(lastNode));
            }
        }

        [TestMethod]
        public void ShortestPathTest()
        {
            List<Node> nodes = new List<Node>();
            Node a = new Node(random, 0, 15);
            Node b = new Node(random, 1, 15);
            Node c = new Node(random, 2, 15);
            Node d = new Node(random, 3, 15);
            Node e = new Node(random, 4, 15);

            a.AddGate(Exit.Right, b);
            a.AddGate(Exit.Left, c);
            b.AddGate(Exit.Left, a);
            b.AddGate(Exit.Right, c);
            b.AddGate(Exit.Top, e);
            c.AddGate(Exit.Top, a);
            c.AddGate(Exit.Left, b);
            c.AddGate(Exit.Right, d);
            d.AddGate(Exit.Left, c);
            e.AddGate(Exit.Left, b);

            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(c);
            nodes.Add(d);
            nodes.Add(e);

            Dungeon dungeon = new Dungeon(random, nodes, 1, 15);
            List<Node> shortPath = dungeon.ShortestPath(d, e);
            List<Node> path = new List<Node>();
            path.Add(c);
            path.Add(b);
            path.Add(e);
            Assert.IsNotNull(shortPath);

            for (int i = 0; i < 3; i++)
                Assert.IsTrue(shortPath[i] == path[i]);

            Node f = new Node(random, 5, 15);

            shortPath = dungeon.ShortestPath(f, a);
            Assert.IsNull(shortPath);

            shortPath = dungeon.ShortestPath(a, f);
            Assert.IsNull(shortPath);

            shortPath = dungeon.ShortestPath(a, a);
            Assert.IsTrue(shortPath.Count == 0);
        }

        [TestMethod]
        public void PathDoesNotExistTest()
        {
            List<Node> nodes = new List<Node>();
            Node a = new Node(random, 0, 15);
            Node b = new Node(random, 1, 15);
            Node c = new Node(random, 2, 15);
            Node d = new Node(random, 3, 15);
            Node e = new Node(random, 4, 15);

            a.AddGate(Exit.Right, b);
            a.AddGate(Exit.Left, c);
            b.AddGate(Exit.Left, a);
            b.AddGate(Exit.Right, c);
            c.AddGate(Exit.Top, a);
            c.AddGate(Exit.Left, b);
            c.AddGate(Exit.Right, d);
            d.AddGate(Exit.Left, c);

            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(c);
            nodes.Add(d);
            nodes.Add(e);

            Dungeon dungeon = new Dungeon(random, nodes, 1, 15);
            List<Node> shortPath = dungeon.ShortestPath(d, e);

            Assert.IsNull(shortPath);
        }

        [TestMethod]
        public void DungeonToStringTest()
        {
            List<Node> nodes = new List<Node>();
            Node a = new Node(random, 0, 15);
            Bridge b = new Bridge(random, 1, 15, 1);
            Node c = new Node(random, 2, 15);
            Bridge d = new Bridge(random, 3, 15, 2);
            Node e = new Node(random, 4, 15);

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

            Player player = new Player(100);
            b.Player = player;
            b.locked = false;
            Pack p = new Pack(2);
            p.Enemies.Add(new Enemy("", 'a', 10));
            p.Enemies.Add(new Enemy("", 'b', 10));
            c.AddPack(p);
            p = new Pack(1);
            p.Enemies.Add(new Enemy("", 'c', 10));
            d.AddPack(p);
            d.AddPack(p);
            Dungeon dungeon = new Dungeon(random, nodes, 1, 15);
            Assert.AreEqual(
                "N0(1,)[]" + "\n" +
                ">B1(0,2,)[]" + "\n" +
                "N2(1,3,)[(ab)]" + "\n" +
                "!B3(2,4,)[(c)(c)]" + "\n" +
                "N4(3,)[]" + "\n",
                dungeon.ToString());
        }
    }
}
