using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class MonsterCreatorTest
    {
        Random r = new Random();

        [TestMethod]
        [Timeout(3000)]
        public void ConstructorTest()//tests if the constructor creates a working object and if the maxpacksize is not below 1
        {
            for (int i = 0; i < 100; i++)
            {
                //Arrange
                int maximumPackSize = r.Next(int.MinValue,int.MaxValue);
                //Act
                MonsterCreator M = new MonsterCreator(r, maximumPackSize);
                //Assert
                Assert.IsNotNull(M);
                Assert.IsTrue(M.maximumPackSize > 0);
            }
        }

        [TestMethod]
        public void NameGenTest()//asset that the string returned by the generator is indeed a string and not null
        {
            //Arrange
            MonsterCreator M = new MonsterCreator(r,10);
            //Act
            string s = M.GenerateName();
            //Assert
            Assert.IsInstanceOfType(s,typeof(string));
            Assert.IsNotNull(s);
        }

        [TestMethod]
        [Timeout (3000)]
        public void MonsterGenTest()
        {
            //Arrange
            MonsterCreator M = new MonsterCreator(r, 10);

            for(int i =0; i < 100;i++)
            {
                //Act
                Enemy e = M.CreateMonster(r.Next(int.MinValue,int.MaxValue));
                //Assert
                Assert.IsNotNull(e);
                Assert.IsTrue(e.MaxHP >= 1);
                Assert.IsTrue(e.MaxHP >= e.CurrentHP);
            }


        }

        [TestMethod]
        [Timeout(3000)]
        public void GeneratePackTest()
        {
            //Arrange
            MonsterCreator M = new MonsterCreator(r, 10);
            //Act
            Pack P = M.GeneratePack(50);
            //Assert
            Assert.IsNotNull(P);
            Assert.IsTrue(P.Enemies.Count >= 1);
            Assert.IsTrue(P.Enemies.Count <= 10);
            Assert.IsTrue(P.Enemies.Count <= P.Enemies.Capacity);
            
        }
    }
}
