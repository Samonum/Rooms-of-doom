﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using RoomsOfDoom.Items;
using System.Linq;

namespace RoomsOfDoom
{
    public class GameManager
    {
        private bool debug = false;
        private bool randomDebug = false;
        private const int doorsize = 3;
        public Random random;

        public const int Width = 37, Height = 25;
        private Exit exits;
        private Node node;

        private Player player;

        bool inCombat;


        private DungeonCreator dungeonCreator;
        public Dungeon dungeon;
        public int difficulty;
        public bool acceptinput;
        private Logger logger;

        public int seed = -1;
        public int basePackCount = 10, maxCapacity = 10;


        public GameManager(bool acceptinput = true, Random random = null)
        {
            this.acceptinput = acceptinput;

            LetsBoogy();
            StartFirstLevel(random);
        }

        public void StartFirstLevel(Random random)
        {
            difficulty = 1;
            bool inMenu = true;
            while (inMenu)
            {
                try
                {
                    Console.Clear();
                }
                catch (Exception e)
                {
                }
                Console.WriteLine("You will soon be entering dungeon level {0}", difficulty);
                Console.WriteLine("to load an old save press 'l', to load a replay press 'r' or press 'c' to conitnue");

                char input = 'c';
                if (acceptinput)
                    input = Console.ReadKey().KeyChar;

                switch (input)
                {
                    case 'r':
                    case 'R':
                        Console.WriteLine("What replay do you wish to load?");
                        new Log(this, Console.ReadLine()).PlayReplay(100);
                        break;
                    case 'l':
                    case 'L':
                        Console.WriteLine("What savefile would you like to load?");
                        inMenu = !LoadGame(Console.ReadLine());
                        if (!inMenu)
                            CreateDungeon();
                        break;
                    case 'c':
                    case 'C':
                        inMenu = false;
                        Initialize(random);
                        break;
                    case 's':
                    case 'S':
                        Seed();
                        break;
                }
            }
        }
        
        public void Seed()
        {
            Console.WriteLine("Here you can enter your dungeon creation settings. These settings are saved untill you restart the game.");
            Console.WriteLine("Enter the seed to be used for the random number generator, leave empty to use a random seed.");
            string s = Console.ReadLine();
            seed = -1;
            int.TryParse(s, out seed);
            Console.WriteLine("The following settings will make it impossible to create a replay of your game.");
            Console.WriteLine("Enter the difficulty level at which you wish to start.");
            s = Console.ReadLine();
            difficulty = 1;
            int.TryParse(s, out difficulty);
            Console.WriteLine("Enter the base pack count the game would start with at difficulty level 1, pack count scales up with the difficulty.");
            s = Console.ReadLine();
            basePackCount = 10;
            int.TryParse(s, out basePackCount);
            Console.WriteLine("Enter the amount of enemies that is allowed in each normal room.");
            s = Console.ReadLine();
            maxCapacity = 10;
            int.TryParse(s, out maxCapacity);
        }


        public void Initialize(Random random)
        {
            logger = new Logger();

            this.random = random;
            if (this.random == null)
            {
                int lseed = seed;
                if (seed == -1)
                {
                    random = new Random();
                    lseed = random.Next();
                }
                this.random = new DebugableRandom(lseed);
                logger.WriteLine(lseed);
            }


            player = new Player();
            CreateDungeon();
        }

        public void StartNextLevel()
        {
            difficulty++;
            bool inMenu = true;
            while (inMenu)
            {
                try
                {
                    Console.Clear();
                }
                catch (Exception e)
                {
                }
                Console.WriteLine("You will soon be entering dungeon level {0}", difficulty);
                Console.WriteLine("If you wish to save press s, to load an old save press l or c to conitnue");

                char input = 'c';
                if (acceptinput)
                    input = Console.ReadKey().KeyChar;
                if (!acceptinput)
                    Thread.Sleep(100);

                switch (input)
                {
                    case 's':
                    case 'S':
                        Console.WriteLine("How would you like to Call your Save?");
                        SaveGame(Console.ReadLine());
                        break;
                    case 'l':
                    case 'L':
                        Console.WriteLine("What savefile would you like to load?");
                        inMenu = !LoadGame(Console.ReadLine());
                        break;
                    case 'c':
                    case 'C':
                        inMenu = false;
                        break;
                }
            }
            CreateDungeon();
        }

        public void ChangeRooms(Node newNode)
        {
            dungeon.PlayerNode = newNode;
            CurrentNode.Player = null;
            newNode.Player = player;
            dungeon.MacroUpdate();
            InitRoom(newNode);
        }

        public void InitRoom(Node newNode)
        {
            Exit entrance = 0;
            foreach (KeyValuePair<Exit, Node> n in newNode.AdjacencyList)
                if (n.Value == node)
                    entrance = n.Key;

            this.node = newNode;
            player.ScoreMultiplier = node.Multiplier * (1 + (difficulty -1) / 2);

            exits = 0;
            foreach (KeyValuePair<Exit, Node> exit in node.AdjacencyList)
                exits |= exit.Key;

            inCombat = node.CurrentPack != null;

            node.PlaceEnemies();
            PlacePlayer(entrance);
        }

        public void PlacePlayer(Exit entrance)
        {
            switch (entrance)
            {
                case Exit.Top:
                    player.Location = new Point(node.TopExit, 2);
                    break;
                case Exit.Bot:
                    player.Location = new Point(node.BotExit, Height - 3);
                    break;
                case Exit.Right:
                    player.Location = new Point(Width - 3, node.RightExit);
                    break;
                case Exit.Left:
                    player.Location = new Point(2, node.LeftExit);
                    break;
                default:
                    player.Location = GetRandomLocation(4);
                    if (node.CurrentPack == null)
                        break;
                    for (int i = 0; i < node.CurrentPack.Size; i++)
                    {
                        if (node.CurrentPack[i].Location == player.Location)
                        {
                            i = 0;
                            player.Location = GetRandomLocation(3);
                            break;
                        }
                    }
                    break;
            }
        }

        public bool HandleCombatRound(char input)
        {
            bool act = true;
            switch (input)
            {
                //TODO: Move this to node?
                
                case 'w':
                case 'W':
                    if (!player.Move(Direction.Up, node.CurrentPack) &&
                        node.WithinTopGate(player.Location.X))
                            ChangeRooms(node.AdjacencyList[Exit.Top]);
                    break;
                case 'a':
                case 'A':
                    if (!player.Move(Direction.Left, node.CurrentPack) &&
                        node.WithinLeftGate(player.Location.Y))
                            ChangeRooms(node.AdjacencyList[Exit.Left]);
                    break;
                case 's':
                case 'S':
                    if (!player.Move(Direction.Down, node.CurrentPack) &&
                        node.WithinBotGate(player.Location.X))
                            ChangeRooms(node.AdjacencyList[Exit.Bot]);
                    break;
                case 'd':
                case 'D':
                    if (!player.Move(Direction.Right, node.CurrentPack) &&
                        node.WithinRightGate(player.Location.Y))
                            ChangeRooms(node.AdjacencyList[Exit.Right]);
                    break;
                case '1': 
                    player.UseItem(new Potion(), dungeon);
                    break;
                case '2': 
                    player.UseItem(new TimeCrystal(), dungeon);
                    break;
                case '3':
                    player.UseItem(new MagicScroll(random, this), dungeon);
                    break;
                case '4':
                    player.UseItem(new LevelKey(this), dungeon);
                    break;
                case 'e':
                case 'E':
                    break;
                case 'z':
                case 'Z':
                    debug = !debug;
                    act = false;
                    break;
                case 'x':
                case 'X':
                    if (debug)
                    {
                        dungeon.MacroUpdate();
                        node.PlaceEnemies();
                    }
                    break;
                case '!':
                    act = false;
                    if (debug)
                        player.AddItem(new Loot(0, '\n'));
                    break;
                case '@':
                    act = false;
                    if (debug)
                        player.AddItem(new Loot(1, '\n'));
                    break;
                case '#':
                    act = false;
                    if (debug)
                        player.AddItem(new Loot(2, '\n'));
                    break;
                case '$':
                    act = false;
                    if (debug)
                        player.AddItem(new Loot(3, '\n'));
                    break;
                case 'r':
                case 'R':
                    randomDebug = !randomDebug;
                    act = false;
                    break;
                default:
                    act = false;
                    break;
            }
            return act;
        }

        //Move to node?
        public void TryPickUpLoot()
        {
            List<Loot> items = node.lootList;
            for(int i = 0; i < node.lootList.Count; i++)
                if (player.Location == items[i].Location)
                {
                    player.AddItem(items[i]);
                    items.RemoveAt(i);
                }
        }

        public void GameOver()
        {
            try
            {
                Console.Clear();
            }
            catch (Exception)
            {
            }
            Console.WriteLine();
            Console.WriteLine(" YOU LOSE!");
            Console.WriteLine(" We're very sorry and wish you all the best in your next adventure.");

            HighScores highscores = new HighScores();
            Console.WriteLine("Your score is: " + player.GetScore + " ! Please enter your name:");
            if(acceptinput)
                highscores.EnterHighScore(player.GetScore, Console.ReadLine());
            highscores.displayHighScores();
            if (acceptinput)
            {
                Console.WriteLine("Type the name under which you wish to save the replay. Leave empty to not save the replay.");
                while (!SaveReplay(Console.ReadLine())) ;
                Console.WriteLine(" Press any key to resurrect yourself and lose all your points and items. Yeah, you could chose not to, but then you would remain death.");
                Console.ReadKey();
            }
            difficulty = 0;
            player = new Player();
            StartFirstLevel(null);
        }

        public void UpdateEnemies()
        {
            node.MicroUpdates();
        }

        public Point GetRandomLocation(int distFromWall)
        {
            return new Point(random.Next(Width - distFromWall - 2) + 1 + distFromWall / 2,
                random.Next(Height - distFromWall - 2) + 1 + distFromWall / 2);
        }

        public Node CurrentNode
        {
            get { return node; }
        }

        public void Update(char testinput = '')
        {
            Draw();
            if (HandleInput(testinput))
            {
                TryPickUpLoot();
                UpdateEnemies();
                if (!player.Alive)
                    GameOver();
                player.UpdateItems();
            }
        }

        public bool HandleInput(char testinput = '')
        {
            if (testinput != '')
                return HandleCombatRound(testinput);

            if (!acceptinput)
                return HandleCombatRound('e');

            char input = Console.ReadKey().KeyChar;
            logger.Write(input);
            return HandleCombatRound(input);
        }

        public Player GetPlayer
        {
            get { return player; }
        }

        public void CreateDungeon()
        {
            dungeonCreator = new DungeonCreator(random);
            float halfPackCount = (basePackCount / 2f);
            dungeon = dungeonCreator.CreateDungeon(difficulty, (int)(halfPackCount + difficulty * halfPackCount), maxCapacity + maxCapacity * (difficulty - 1) / 3);
            dungeon.nodes[0].Player = GetPlayer;
            dungeon.PlayerNode = dungeon.nodes[0];
            ItemGenerator.Init(random, dungeon, player);
            InitRoom(dungeon.nodes[0]);
        }

        public string FormatHud()
        {
            return String.Format(
@" ________________________________________________ 
/                                                \
| HP: {0}       POINTS:{1}    KEY:   |
| POT: {2}       TC: {3}       MS: {4}     {5}   |
\________________________________________________/ ",

new String[] { player.CurrentHP.ToString().PadLeft(4), player.GetScore.ToString().PadLeft(14), 
    player.GetPotCount.ToString().PadLeft(3), player.GetCrystalCount.ToString().PadLeft(3), player.GetScrollCount.ToString().PadLeft(3), 
    player.inventory[3] >= 1 ? "GET" : "   "});
        }

        public void Draw()
        {
            try
            {
                Console.Clear();
            }
            catch (Exception e)
            {
            }
            
            if (CurrentNode.isBridge())
                Console.ForegroundColor = ConsoleColor.Red;
            else if (CurrentNode.IsExit)
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            else
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            string[] drawmap = CurrentNode.CreateEnemyOverview(debug);
            foreach (string s in drawmap)
                Console.WriteLine(s);
            Console.WriteLine(FormatHud());
            if (debug)
            {
                Console.WriteLine(dungeon.ToString());
                DebugableRandom r = (DebugableRandom)random;
                if (r != null)
                {
                    Console.WriteLine("Seed: " + r.initialSeed);
                    Console.WriteLine("Call count: " + r.callCount);
                }
            }
        }

        public bool SaveGame(string fileName)
        {
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || fileName == "")
            {
                Console.WriteLine("Your filename contains illegal characters. Press any Key to return.");
                if(acceptinput)
                    Console.ReadKey();
                return false;
            }

            logger.Save(fileName, true);

            if (File.Exists(fileName))
            {
                Console.WriteLine("There already is a save with that name. Do you want to overwrite? [y/n]");
                if (acceptinput)
                    if (Console.ReadKey().KeyChar != 'y')
                    {
                        Console.WriteLine("Did not save file.");
                        Console.ReadKey();
                        return false;
                    }
            }

            Player p = GetPlayer;
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(
                    p.CurrentHP + ";" +
                    p.GetScore + ";" +
                    p.GetPotCount + ";" +
                    p.GetCrystalCount + ";" +
                    p.GetScrollCount + ";" +
                    difficulty
                );
            }

            Console.WriteLine("saved file: {0}", fileName);
            if(acceptinput)
                Console.ReadKey();
            return true;
        }

        public bool LoadGame(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File Does Not Exist. Press any Key to return.");
                if(acceptinput)
                    Console.ReadKey();
                return false;
            }
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    string[] data = line.Split(';');
                    player = new Player(int.Parse(data[0]));
                    player.IncreaseScore(int.Parse(data[1]));
                    player.SetItems(
                        byte.Parse(data[2]),
                        byte.Parse(data[3]),
                        byte.Parse(data[4])
                    );
                    difficulty = int.Parse(data[5]);
                }
            }
            logger = new Logger();
            logger.Load(fileName);
            if (random == null)
                random = new Random();
            int seed = random.Next();
            logger.Write("\n" + seed + "\n");
            random = new DebugableRandom(seed);
            return true;
        }


        public bool SaveReplay(string fileName)
        {
            if (fileName == null)
                return true;

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Console.WriteLine("Your filename contains illegal characters. Press any Key to return.");
                if (acceptinput)
                    Console.ReadKey();
                return false;
            }

            if (File.Exists(fileName + ".play"))
            {
                Console.WriteLine("There already is a save with that name. Do you want to overwrite? [y/n]");
                if (acceptinput)
                    if (Console.ReadKey().KeyChar != 'y')
                    {
                        Console.WriteLine("Did not save file.");
                        Console.ReadKey();
                        return false;
                    }
            }

            logger.Save(fileName, false);

            return true;
        }

        public void LetsBoogy()
        {
            new Task(() =>
            {
                Random r = new Random();
                MusicDictionary Music = new MusicDictionary();
                while (true)
                {
                    if (this.node != null && this.node.isBridge())
                    {
                        Thread.Sleep(60);
                        Console.Beep(Music.NoteArrayBlues[r.Next(0, 6)], 100);
                    }
                    else if (this.node != null && this.node.IsExit)
                    {
                        Thread.Sleep(180);
                        Console.Beep(Music.NoteArrayBlues[r.Next(0, 6)], 150);
                    }
                    else
                    {
                        Thread.Sleep(120);
                        Console.Beep(Music.NoteArray[r.Next(0, 5)], 100);
                    }
                }
            }).Start();
        }
    }
}