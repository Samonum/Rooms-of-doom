using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class PackTest
    {
        Random r = new Random();

        [TestMethod]
        public void FullPackTest()
        {
            for (int i = 0; i < 100; i++)
            {
                //Arrange
                Pack P = new Pack(r.Next(int.MinValue, int.MaxValue));
                Enemy e = new Enemy("Giant Goblin", 30);
                //Act
                for (int j = 0; j < 1000; j++)
                {
                    P.Add(e);
                }
                //Assert
                Assert.IsNotNull(P);
                Assert.IsTrue(P.Enemies.Capacity >= P.Enemies.Count);
                Assert.IsTrue(P.Enemies.Contains(e));
            }

        }
    }
}
