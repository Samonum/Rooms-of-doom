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
            testSubject.IncreaseScore(int.MaxValue);
            Assert.AreEqual(testSubject.GetScore, int.MaxValue);
            testSubject.IncreaseScore(1000);
            Assert.AreEqual(testSubject.GetScore, int.MaxValue);
            HudTest();
            try
            {
                testSubject.IncreaseScore(-1);
                Assert.Fail("Didn't crash");
            }
            catch (ArgumentOutOfRangeException e)
            { }
            catch(Exception e)
            {
                Assert.Fail("Threw wrong error");
            }
        }

        [TestMethod]
        public void ScreenWidthTest()
        {
            string[] screen = testSubject.CreateEnemyOverview();
            foreach(string s in screen)
                Assert.IsTrue(s.Length <= Console.WindowWidth);
        }

        [TestMethod]
        public void CompleteHudTest()
        {
            for (int i = 0; i <= 11; i++)
            {
                PlayerTest playerTest = new PlayerTest();
                playerTest.Init();
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
