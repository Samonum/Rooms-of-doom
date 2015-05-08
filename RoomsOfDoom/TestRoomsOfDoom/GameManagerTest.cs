using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Drawing;

namespace TestRoomsOfDoom
{

    [TestClass]
    public class GameManagerTest
    {
        Random random;
        static GameManager testSubject;
        public GameManagerTest()
        {
            random = new Random();
        }

        [ClassInitialize]
        public static void ClassInit(TestContext t)
        {
            testSubject = new GameManager(false);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            testSubject.GameOver();
        }

        [TestMethod]
        public void StartState()
        {
            Assert.AreEqual(0, testSubject.GetPlayer.GetScore);
            Assert.AreEqual(1, testSubject.difficulty);
            Assert.AreEqual(2, testSubject.GetPlayer.GetPotCount);
            Assert.AreEqual(2, testSubject.GetPlayer.GetCrystalCount);
            Assert.AreEqual(2, testSubject.GetPlayer.GetScrollCount);
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
                int increase = random.Next(1000);
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
                testSubject.GetPlayer.Hit(9 * i);
                playerTest.ScrollTest();
                playerTest.PotionTest();
                playerTest.CrystalTest();
                HudTest();
            }
        }

        [TestMethod]
        public void EnemyAdditionTest()
        {
            MonsterCreator creator = new MonsterCreator(new NotSoRandom(25), 25);
            Pack p = creator.GeneratePack(9001);
            Node node = new Node(random, 1, 15);
            node.AddPack(p);
            for (int n = 0; n < 100; n++)
            {
                testSubject.InitRoom(node);
                for (int i = 0; i < p.Size; i++)
                    for (int j = 0; j < i; j++)
                        Assert.AreNotEqual(p[i].Location, p[j].Location, "Two enemies at the same place. The horror!");
            }
        }

        [TestMethod]
        public void MoveRoomAndPlacePlayerTest()
        {
            Node oldNode = testSubject.CurrentNode;
            Exit exit = (Exit)1;
            while (!oldNode.AdjacencyList.ContainsKey(exit))
                Assert.IsTrue((int)(exit = (Exit)((int)exit * 2)) <= 8);
            Node newNode = oldNode.AdjacencyList[exit];
            testSubject.PlacePlayer(exit);
            switch(exit)
            {
                case Exit.Top:
                    testSubject.HandleCombatRound('w');
                    testSubject.HandleCombatRound('w');
                    break;
                case Exit.Bot:
                    testSubject.HandleCombatRound('s');
                    testSubject.HandleCombatRound('s');
                    break;
                case Exit.Left:
                    testSubject.HandleCombatRound('a');
                    testSubject.HandleCombatRound('a');
                    break;
                case Exit.Right:
                    testSubject.HandleCombatRound('d');
                    testSubject.HandleCombatRound('d');
                    break;
            }

            Assert.AreEqual(newNode, testSubject.CurrentNode);

            Player p = testSubject.GetPlayer; 
            if(p.Location.X == 2)
            {
                Assert.AreEqual(testSubject.leftExit, p.Location.Y);
                Assert.AreEqual(testSubject.CurrentNode.AdjacencyList[Exit.Left], oldNode);
                return;
            }
            if (p.Location.Y == 2)
            {
                Assert.AreEqual(testSubject.topExit, p.Location.X);
                Assert.AreEqual(testSubject.CurrentNode.AdjacencyList[Exit.Top], oldNode);
                return;
            }
            if (p.Location.X == GameManager.Width - 3)
            {
                Assert.AreEqual(testSubject.rightExit, p.Location.Y);
                Assert.AreEqual(testSubject.CurrentNode.AdjacencyList[Exit.Right], oldNode);
                return;
            }
            if (p.Location.Y == GameManager.Height - 3)
            {
                Assert.AreEqual(testSubject.botExit, p.Location.X);
                Assert.AreEqual(testSubject.CurrentNode.AdjacencyList[Exit.Bot], oldNode);
                return;
            }
                Assert.Fail("Not at an exit");
        }

        [TestMethod]
        public void EnemyUpdateTest()
        {
            Node n = new Bridge(new Random(), 9, 200, 9);
            MonsterCreator creator = new MonsterCreator(random, 1);
            Pack p = creator.GeneratePack(1);
            n.AddPack(p);
            testSubject.InitRoom(n);
            testSubject.GetPlayer.Location = new Point(2, 2);
            p[0].Location = new Point(2, 4);
            testSubject.Update('e');
            Assert.AreEqual(p[0].Location, new Point(2, 3));
            testSubject.Update('e');
            Assert.AreEqual(testSubject.GetPlayer.MaxHP - 1, testSubject.GetPlayer.CurrentHP);
            int max = p[0].MaxHP;
            testSubject.Update('s');
            if (max <= Player.strength)
                Assert.AreEqual(0, p.Size);
            else
                Assert.AreEqual(p[0].MaxHP - Player.strength, p[0].CurrentHP);
        }

        [TestMethod]
        public void DeathTest()
        {
            MakeAbsurd();
            testSubject.GameOver();
            StartState();
        }

        [TestMethod]
        public void SaveTest()
        {
            Assert.IsFalse(testSubject.Save(""), "No empty string saves");
            Assert.IsFalse(testSubject.Save(new string(Path.GetInvalidFileNameChars())), "No bad names");
            string filename = ".testSave.donotmake.willberemoved";

            MakeAbsurd();
            if (File.Exists(filename))
                File.Delete(filename);

            Assert.IsFalse(testSubject.Load(filename));

            Assert.IsTrue(testSubject.Save(filename));
            testSubject.GameOver();

            Assert.IsTrue(testSubject.Load(filename));
            IsAbsurd();

            File.Delete(filename);
        }

        public void MakeAbsurd()
        {
            testSubject.GetPlayer.IncreaseScore(1000);
            testSubject.GetPlayer.SetItems(12, 13, 14);
            testSubject.difficulty = 9001;
        }

        public void IsAbsurd()
        {
            Assert.AreEqual(1000, testSubject.GetPlayer.GetScore);
            Assert.AreEqual(9001, testSubject.difficulty);
            Assert.AreEqual(12, testSubject.GetPlayer.GetPotCount);
            Assert.AreEqual(13, testSubject.GetPlayer.GetCrystalCount);
            Assert.AreEqual(14, testSubject.GetPlayer.GetScrollCount);
        }

    }
}
