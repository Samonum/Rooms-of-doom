using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class MagicTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // arrange
            double beginMana = 11.99;
            double castAmount = 4.55;
            double expectedMana = 7.44;
            Magic magic = new Magic(beginMana);

            // act
            magic.Cast(castAmount);

            // assert
            double actualMana = magic.mana;
            Assert.AreEqual(expectedMana, actualMana, 0.001, "Account not debited correctly");
        }
    }
}
