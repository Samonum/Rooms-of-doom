using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomsOfDoom;
using RoomsOfDoom.Items;
using System.Collections.Generic;
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

        [TestInitialize]
        public void TestInit()
        {
            testSubject = new GameManager(false);
            random = new Random();
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
            testSubject.CurrentNode.Player = testSubject.GetPlayer;
            string[] screen = testSubject.CurrentNode.CreateEnemyOverview();
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
            Node node = new Node(random, 1024, 128);
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
            for (int i = 1; i <= 4; i++)
            {
                testSubject = new GameManager(false);
                Node oldNode = testSubject.CurrentNode;
                Exit exit = (Exit)1;
                foreach (KeyValuePair<Exit, Node> kvp in oldNode.AdjacencyList)
                {
                    testSubject.ChangeRooms(oldNode);
                    exit = kvp.Key;
                    Node newNode = oldNode.AdjacencyList[exit];
                    testSubject.PlacePlayer(exit);
                    switch (exit)
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
                    if (p.Location.X == 2)
                    {
                        Assert.AreEqual(testSubject.CurrentNode.LeftExit, p.Location.Y);
                        Assert.AreEqual(testSubject.CurrentNode.AdjacencyList[Exit.Left], oldNode);
                        continue;
                    }
                    if (p.Location.Y == 2)
                    {
                        Assert.AreEqual(testSubject.CurrentNode.TopExit, p.Location.X);
                        Assert.AreEqual(testSubject.CurrentNode.AdjacencyList[Exit.Top], oldNode);
                        continue;
                    }
                    if (p.Location.X == GameManager.Width - 3)
                    {
                        Assert.AreEqual(testSubject.CurrentNode.RightExit, p.Location.Y);
                        Assert.AreEqual(testSubject.CurrentNode.AdjacencyList[Exit.Right], oldNode);
                        continue;
                    }
                    if (p.Location.Y == GameManager.Height - 3)
                    {
                        Assert.AreEqual(testSubject.CurrentNode.BotExit, p.Location.X);
                        Assert.AreEqual(testSubject.CurrentNode.AdjacencyList[Exit.Bot], oldNode);
                        continue;
                    }
                    Assert.Fail("Not at an exit");
                }
            }
        }

        [TestMethod]
        public void EnemyUpdateTest()
        {
            Node n = new Bridge(new Random(), 9, 200, 9);
            n.Player = testSubject.GetPlayer;
            MonsterCreator creator = new MonsterCreator(random, 1);
            Pack p = creator.GeneratePack(1);
            n.AddPack(p);
            testSubject.InitRoom(n);
            testSubject.GetPlayer.Location = new Point(2, 2);
            p[0].Location = new Point(2, 4);
            testSubject.Update('e');
            Assert.AreEqual(p[0].Location, new Point(2, 3));
            testSubject.Update('e');
            Assert.AreEqual(testSubject.GetPlayer.MaxHP - p[0].damage, testSubject.GetPlayer.CurrentHP);
            int max = p[0].MaxHP;
            testSubject.Update('s');
            if (max <= Player.strength)
                Assert.AreEqual(0, p.Size);
            else
                Assert.AreEqual(p[0].MaxHP - Player.strength, p[0].CurrentHP);
        }

        [TestMethod]
        public void RandomConsistencyTest()
        {
            testSubject.Initialize(null);
            RandomConsistency();
        }

        [TestMethod]
        public void LootKeyCheck() 
        {
            // TODO: This test breaks completely as it does not follow new rules
            int curdif = testSubject.difficulty;
            foreach (Node n in testSubject.dungeon.nodes)
                if (n.IsExit)
                {
                    testSubject.InitRoom(n);
                    n.Player = testSubject.GetPlayer;
                }
            Assert.IsInstanceOfType(testSubject.CurrentNode.lootList[0], typeof(Loot));
            Assert.AreEqual(testSubject.CurrentNode.lootList[0].ID, 3);
            testSubject.CurrentNode.lootList[0].Location = new Point(testSubject.GetPlayer.Location.X - 1, testSubject.GetPlayer.Location.Y);
            if (testSubject.CurrentNode.CurrentPack != null)
                foreach (Enemy e in testSubject.CurrentNode.CurrentPack)
                    if (e.Location == testSubject.CurrentNode.lootList[0].Location)
                        e.Location = new Point();
            testSubject.Update('a');
            Assert.AreEqual(1, testSubject.GetPlayer.inventory[3]);
            testSubject.Update('4');
            Assert.AreEqual(curdif + 1, testSubject.difficulty);
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
            Assert.IsFalse(testSubject.SaveGame(""), "No empty string saves");
            Assert.IsFalse(testSubject.SaveGame(new string(Path.GetInvalidFileNameChars())), "No bad names");
            string filename = ".testSave.donotmake.willberemoved";

            MakeAbsurd();
            if (File.Exists(filename))
                File.Delete(filename);

            Assert.IsFalse(testSubject.LoadGame(filename));

            Assert.IsTrue(testSubject.SaveGame(filename));
            testSubject.GameOver();

            Assert.IsTrue(testSubject.LoadGame(filename));
            IsAbsurd();
            testSubject.basePackCount = 1;
            testSubject.CreateDungeon();
            RandomConsistency();

            File.Delete(filename);
        }

        [TestMethod]
        public void NonexistingLogTest()
        {
            if (File.Exists("ShouldNotBe.play"))
                File.Delete("ShouldNotBe.play");
            Log log = new Log(testSubject, "ShouldNotBe.play");
            Assert.IsTrue(log.Finished());
            log.Initialize();
            Assert.IsTrue(log.Finished());
            log.CleanUp();
        }

        [TestMethod]
        public void LogItemCheatAndUseTest()
        {
            Log log = new Log(testSubject, ".DO.NOT.TOUCH.test");
            log.Initialize();
            Assert.IsNotNull(log.replay);

            byte[] inventory = (byte[])testSubject.GetPlayer.inventory.Clone();

            log.PlayReplay(0);

            for (int i = 0; i < inventory.Length; i++)
            {
                Assert.AreEqual(testSubject.GetPlayer.inventory[i], inventory[i]);
            }

            inventory = (byte[])testSubject.GetPlayer.inventory.Clone();

            log.PlayStep();
            for(int i = 0; i < inventory.Length; i++)
            {
                Assert.AreEqual(testSubject.GetPlayer.inventory[i], inventory[i]);
            }
        }

        [TestMethod]
        public void LogLevelUpAndLoadedTest()
        {
            Log log = new Log(testSubject, "test");
            log.Initialize();
            int difficulty = 0;
            int lastDifficulty = 0;
            int score = 0;
            while (!log.Finished())
            {
                if (testSubject.difficulty > difficulty)
                {
                    difficulty++;
                    RandomConsistency();
                }

                Assert.IsTrue(testSubject.GetPlayer.GetScore >= score);
                score = testSubject.GetPlayer.GetScore;
                Assert.AreEqual(difficulty, testSubject.difficulty);
                Assert.IsTrue(testSubject.GetPlayer.Alive);
                lastDifficulty = difficulty;
                log.PlayStep();
            }
            Assert.AreEqual(2, lastDifficulty);
        }

        [TestMethod]
        public void HighScoresTest()
        {
            File.Copy("HighScores.testfile", "HighScores.txt", true);
            HighScores scores = new HighScores();
            Tuple<int, string>[] values = scores.LoadScores();
            for (int i = 1; i < values.Length; i++)
            {
                Assert.IsTrue(values[i - 1].Item1 >= values[i].Item1);
            }
            scores.EnterHighScore(150, "");
            scores.EnterHighScore(350, "1337 h4x0r");
            scores.EnterHighScore(150, "");
            values = scores.LoadScores();
            for (int i = 1; i < values.Length; i++)
            {
                Assert.IsTrue(values[i - 1].Item1 >= values[i].Item1);
            }
        }

        public void MakeAbsurd()
        {
            testSubject.GetPlayer.IncreaseScore(1000);
            testSubject.GetPlayer.SetItems(12, 13, 14);
            testSubject.difficulty = 9;
        }

        public void IsAbsurd()
        {
            Assert.AreEqual(1000, testSubject.GetPlayer.GetScore);
            Assert.AreEqual(9, testSubject.difficulty);
            Assert.AreEqual(12, testSubject.GetPlayer.GetPotCount);
            Assert.AreEqual(13, testSubject.GetPlayer.GetCrystalCount);
            Assert.AreEqual(14, testSubject.GetPlayer.GetScrollCount);
        }
        
        public void RandomConsistency()
        {
            Assert.AreSame(testSubject.random, ItemGenerator.random, "Bad item generator");
            Assert.AreSame(testSubject.random, testSubject.dungeon.random, "Bad Dungeon");
        }

    }
}
