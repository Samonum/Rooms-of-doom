using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

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
            node.Update();
            Assert.IsTrue(node.PackList.Count == 0);

            MonsterCreator mc = new MonsterCreator(random, 6);
            Pack b = mc.GeneratePack(1);
            node.AddPack(b);
            node.Update();
            Assert.IsTrue(node.PackList.Count == 1);
        }
    }
}
