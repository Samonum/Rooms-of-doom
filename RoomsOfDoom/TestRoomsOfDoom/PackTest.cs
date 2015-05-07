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
                int size = r.Next(-1000000,1000000);
                Pack P = new Pack(size);
                Enemy e = new Enemy("Giant Goblin", 30);

                if (size <= 0)
                    size = 1;
                //Act
                for (int j = 0; j < size + 5; j++)
                {
                    P.Add(e);
                }
                //Assert
                Assert.IsNotNull(P);
                Assert.IsTrue(P.Enemies.Capacity >= P.Enemies.Count);
                Assert.IsTrue(P.Enemies.Contains(e));
                if(size > 0)
                    Assert.IsTrue(P.Enemies.Count == size);
            }

        }
    }
}
