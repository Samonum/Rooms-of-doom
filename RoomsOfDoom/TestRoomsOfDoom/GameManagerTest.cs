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
        public void PotionTest()
        {
            int pots = 0;
            int count = 100 + r.Next(50);
            for (int i = 0; i < count; i++)
            {
                testSubject.AddPotion();
                pots++;
            }
            Assert.AreEqual(pots, testSubject.GetPotCount, "Pots don't add up well.");
            HudTest();
        }

        [TestMethod]
        public void CrystalTest()
        {
            int crystals = 0;
            int count = 100 + r.Next(50);
            for (int i = 0; i < count; i++)
            {
                testSubject.AddCrystal();
                crystals++;
            }
            Assert.AreEqual(crystals, testSubject.GetCrystalCount, "Crystals don't add up well.");
            HudTest();
        }

        [TestMethod]
        public void ScrollTest()
        {
            int scrolls = 0;
            int count = 100 + r.Next(50);
            for (int i = 0; i < count; i++)
            {
                testSubject.AddScroll();
                scrolls++;
            }
            Assert.AreEqual(scrolls, testSubject.GetScrollCount, "Scrolls don't add up well.");
            HudTest();
        }

        [TestMethod]
        public void CompleteHudTest()
        {
            for (int i = 0; i < 11; i++)
            {
                testSubject = new GameManager();
                testSubject.GetPlayer.Hit(9 * i);
                ScrollTest();
                PotionTest();
                CrystalTest();
            }
        }
    }
}
