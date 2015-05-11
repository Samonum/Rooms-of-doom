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

            a.AdjacencyList.Add(Exit.Right, b);
            b.AdjacencyList.Add(Exit.Left, a);
            b.AdjacencyList.Add(Exit.Right, c);
            c.AdjacencyList.Add(Exit.Left, b);
            c.AdjacencyList.Add(Exit.Right, d);
            d.AdjacencyList.Add(Exit.Left, c);
            d.AdjacencyList.Add(Exit.Right, e);
            e.AdjacencyList.Add(Exit.Left, d);

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

            a.AdjacencyList.Add(Exit.Right, b);
            a.AdjacencyList.Add(Exit.Left, c);
            b.AdjacencyList.Add(Exit.Left, a);
            b.AdjacencyList.Add(Exit.Right, c);
            b.AdjacencyList.Add(Exit.Top, e);
            c.AdjacencyList.Add(Exit.Top, a);
            c.AdjacencyList.Add(Exit.Left, b);
            c.AdjacencyList.Add(Exit.Right, d);
            d.AdjacencyList.Add(Exit.Left, c);
            e.AdjacencyList.Add(Exit.Left, b);

            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(c);
            nodes.Add(d);
            nodes.Add(e);

            Dungeon dungeon = new Dungeon(random, nodes, 1, 15);
            List<Node> shortPath = dungeon.ShortestPath(d, e);
            List<Node> path = new List<Node>();
            path.Add(d);
            path.Add(c);
            path.Add(b);
            path.Add(e);
            Assert.IsNotNull(shortPath);

            for (int i = 0; i < 4; i++)
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

            a.AdjacencyList.Add(Exit.Right, b);
            a.AdjacencyList.Add(Exit.Left, c);
            b.AdjacencyList.Add(Exit.Left, a);
            b.AdjacencyList.Add(Exit.Right, c);
            c.AdjacencyList.Add(Exit.Top, a);
            c.AdjacencyList.Add(Exit.Left, b);
            c.AdjacencyList.Add(Exit.Right, d);
            d.AdjacencyList.Add(Exit.Left, c);

            nodes.Add(a);
            nodes.Add(b);
            nodes.Add(c);
            nodes.Add(d);
            nodes.Add(e);

            Dungeon dungeon = new Dungeon(random, nodes, 1, 15);
            List<Node> shortPath = dungeon.ShortestPath(d, e);

            Assert.IsNull(shortPath);
        }
    }
}
