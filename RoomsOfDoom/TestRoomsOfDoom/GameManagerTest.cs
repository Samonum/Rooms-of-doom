using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;

namespace TestRoomsOfDoom
{
    [TestClass]
    public class GameManagerTest
    {
        Random r;
        GameManager testSubject;
        public GameManagerTest()
        {
            r = new Random();
            testSubject = new GameManager(10);
        }

        [TestMethod]
        public void HudTest()
        {
            string[] hud = testSubject.FormatHud().Split('\n');
            bool match = true;
            string dimensions = hud[0].Length.ToString();
            for (int i = 1; i < hud.Length; i++)
            {
                match &= hud[i - 1].Length == hud[i].Length;
                dimensions += ", " + hud[i].Length;
            }
            Assert.IsTrue(match, "Hud messed up:\n" + dimensions + "\n" + testSubject.FormatHud());
        }

        //Nothing as important as knowing you won't miss any points
        [TestMethod]
        public void scoreTest()
        {
            int score = 0;
            for(int i = 0; i < 10000; i++)
            {
                int increase = r.Next(1000);
                testSubject.IncreaseScore(increase);
                score += increase;
            }
            Assert.AreEqual(score, testSubject.GetScore, "Scores don't add up well.");
            HudTest();
        }

        [TestMethod]
        public void CompleteHudTest()
        {
            for (int i = 0; i < 11; i++)
            {
                PlayerTest playerTest = new PlayerTest();
                testSubject = new GameManager();
                testSubject.GetPlayer.Hit(9 * i);
                playerTest.ScrollTest();
                playerTest.PotionTest();
                playerTest.CrystalTest();
                HudTest();
            }
        }
    }
}
