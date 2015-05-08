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
            //testSubject = new GameManager(10);
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
            Player player = new Player();
            player.ScoreMultiplier = 1;
            int score = 0;
            for(int i = 0; i < 10000; i++)
            {
                int increase = r.Next(1000);
                player.IncreaseScore(increase);
                score += increase;
            }
            Assert.AreEqual(score, player.GetScore, "Scores don't add up well.");
            player.IncreaseScore(-1);
            Assert.AreEqual(score, player.GetScore, "No more points for you.");
            HudTest();
            player.IncreaseScore(int.MaxValue);
            Assert.AreEqual(player.GetScore, int.MaxValue);
            player.IncreaseScore(1000);
            Assert.AreEqual(player.GetScore, int.MaxValue);
            HudTest();
            player.IncreaseScore(-1);
            Assert.AreEqual(player.GetScore, int.MaxValue);
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
                //testSubject = new GameManager();
                testSubject.GetPlayer.Hit(9 * i);
                playerTest.ScrollTest();
                playerTest.PotionTest();
                playerTest.CrystalTest();
                HudTest();
            }
        }
    }
}
