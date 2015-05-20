using System;
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
        private const int doorsize = 3;
        Random random;

        private char[][] map;
        public const int Width = 37, Height = 25;
        private Exit exits;
        public int topExit, leftExit, rightExit, botExit;
        private Node node;

        public Pack enemies;
        int curPack;//what is this ??? remove?
        private Player player;

        bool inCombat;

        public List<IItem> items;
        ItemGenerator itemGenerator;

        private DungeonCreator dungeonCreator;
        public Dungeon dungeon;
        public int difficulty;
        private bool acceptinput;

        //TODO_FLEE variables
        Pack fledEnemies = new Pack(50); 

        public GameManager(bool testMode = true, Random random = null)
        {

            this.random = random;
            if(random == null)
                this.random = new Random();

            this.acceptinput = testMode;

            difficulty = 0;

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


            player = new Player();
            StartNextLevel();
        }

        public void StartNextLevel()
        {
            difficulty++;
            bool inMenu = true;
            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine("You will soon be entering dungeon level {0}", difficulty);
                Console.WriteLine("If you wish to save press s, to load an old save press l or c to conitnue");

                char input = 'c';
                if(acceptinput)
                    input = Console.ReadKey().KeyChar;

                switch (input)
                {
                    case 's':
                    case 'S':
                        Console.WriteLine("How would you like to Call your Save?");
                        Save(Console.ReadLine());
                        break;
                    case 'l':
                    case 'L':
                        Console.WriteLine("What savefile would you like to load?");
                        Load(Console.ReadLine());
                        break;
                    case 'c':
                    case 'C':
                        inMenu = false;
                        break;
                }
            }
            CreateDungeon(10, 10);
        }

        public void ChangeRooms(Node newNode)
        {
            dungeon.Update();
            InitRoom(newNode);
        }

        public void InitRoom(Node newNode)
        {

            items = new List<IItem>(2);
            LevelKey key = new LevelKey(this);
            key.Location = GetRandomLocation(8);
            if (newNode.IsExit && player.inventory[3] < 1)
                items.Add(key);

            Exit entrance = 0;
            foreach (KeyValuePair<Exit, Node> n in newNode.AdjacencyList)
                if (n.Value == node)
                    entrance = n.Key;

            this.node = newNode;
            player.ScoreMultiplier = node.Multiplier;

            exits = 0;
            foreach (KeyValuePair<Exit, Node> exit in node.AdjacencyList)
                exits |= exit.Key;
            topExit = 10 + random.Next(Width - 20);
            leftExit = 10 + random.Next(Height - 20);
            rightExit = 10 + random.Next(Height - 20);
            botExit = 10 + random.Next(Width - 20);

            if (node.PackList.Count == 0)
            {
                inCombat = false;
                enemies = new Pack(0);
            }
            else
            {
                inCombat = true;
                enemies = node.PackList[0];
            }

            PlaceEnemies(enemies);
            PlacePlayer(entrance);

        }

        public void PlacePlayer(Exit entrance)
        {
            switch (entrance)
            {
                case Exit.Top:
                    player.Location = new Point(topExit, 2);
                    break;
                case Exit.Bot:
                    player.Location = new Point(botExit, Height - 3);
                    break;
                case Exit.Right:
                    player.Location = new Point(Width - 3, rightExit);
                    break;
                case Exit.Left:
                    player.Location = new Point(2, leftExit);
                    break;
                default:
                    player.Location = GetRandomLocation(4);
                    for (int i = 0; i < enemies.Size; i++)
                    {
                        if (enemies[i].Location == player.Location)
                        {
                            i = 0;
                            player.Location = GetRandomLocation(3);
                            break;
                        }
                    }
                    break;
            }
        }

        public void PlaceEnemies(Pack enemies)
        {
            for (int i = 0; i < enemies.Size; i++)
            {
                enemies[i].Location = GetRandomLocation(4);
                for (int j = 0; j < i; j++)
                    if (enemies[i].Location == enemies[j].Location)
                    {
                        i--;
                        break;
                    }
                    else if (enemies[i].Location == player.Location)
                        i--;
            }
        }

        public bool HandleCombatRound(char input)
        {
            bool act = true;
            switch (input)
            {
                case 'w':
                    if (!player.Move(Direction.Up, enemies) &&
                        (exits & Exit.Top) == Exit.Top && 
                            player.Location.X > topExit - doorsize &&
                            player.Location.X < topExit + doorsize)
                                ChangeRooms(node.AdjacencyList[Exit.Top]);
                    break;
                case 'a': 
                    if (!player.Move(Direction.Left, enemies) &&
                        (exits & Exit.Left) == Exit.Left &&
                        player.Location.Y > leftExit - doorsize && 
                            player.Location.Y < leftExit + doorsize)
                                ChangeRooms(node.AdjacencyList[Exit.Left]);
                    break;
                case 's': 
                    if (!player.Move(Direction.Down, enemies) &&
                        (exits & Exit.Bot) == Exit.Bot &&
                        player.Location.X > botExit - doorsize && 
                        player.Location.X < botExit + doorsize)
                                ChangeRooms(node.AdjacencyList[Exit.Bot]);
                    break;
                case 'd': 
                    if (!player.Move(Direction.Right, enemies))
                        if ((exits & Exit.Right) == Exit.Right)
                            if (player.Location.Y > rightExit - doorsize && player.Location.Y < rightExit + doorsize)
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
                    break;
                default:
                    act = false;
                    break;
            }
            return act;
        }

        public void TryPickUpLoot()
        {
            for(int i = 0; i < items.Count; i++)
                if(player.Location == items[i].Location)
                {
                    player.AddItem(items[i]);
                    items.RemoveAt(i);
                }
        }

        public void GameOver()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" YOU LOSE!");
            Console.WriteLine(" We're very sorry and hope you all the best in your next adventure.");

            HighScores highscores = new HighScores();
            highscores.EnterHighScore(player.GetScore);
            highscores.displayHighScores();


            Console.WriteLine(" Press any key to resurrect yourself and lose all your points and items.");
            if(acceptinput)
                Console.ReadKey();
            difficulty = 0;
            player = new Player();
            StartNextLevel();
        }

        public void UpdateEnemies()
        {
            //TODO_FLEE
            //if more than 70% damage has been done to the pack all enemies flee, otherwise they fight to the death
            if(enemies.CurrentPackHP >= (0.3 * enemies.MaxPackHP))
            {
                foreach (Enemy e in enemies)
                {
                    e.Move(player);//move towards the player to attack
                }
            }
            else
            {
                //TODO_FLEE
                //get random door to flee to
                Exit randomExit = node.AdjacencyList.ElementAt(random.Next(0, 0)).Key;
                Point doorLocation = new Point(0, 0);
                switch (randomExit)
                {
                    case Exit.Top:
                        doorLocation = new Point(topExit,1);
                        break;
                    case Exit.Bot:
                        doorLocation = new Point(botExit, Height -3);
                        break;
                    case Exit.Left:
                        doorLocation = new Point(3, leftExit);
                        break;
                    case Exit.Right:
                        doorLocation = new Point(Width - 3, rightExit);
                        break;
                }

                Enemy target = new Enemy("dummy", '?',999);
                target.Location = doorLocation;
                List<Enemy> enemiesToRemove = new List<Enemy>();
                foreach (Enemy e in enemies)
                {
                    //FLY YOU FOOLS! TODO_FLEE
                    //TODO fix bug where enemy.move checks for collision with the player instead of just the target. Or make a new method for this since this method is a bit hacky
                    e.Move(target);
                    //TODO_FLEE add enemies to fleelist removing them from the dungeon
                    if(e.Location == target.Location)
                    {
                        //enemies.Enemies.Remove(e); not allowed to edit collection in a foreach over that collection derp
                        enemiesToRemove.Add(e);
                        fledEnemies.Add(e);
                    }
                }
                foreach(Enemy e in enemiesToRemove)
                {
                    enemies.Enemies.Remove(e);
                }
            }

            

            if (enemies.Size == 0)
            {
                node.RemovePack(enemies);
                Node neighbourExit = node.AdjacencyList.ElementAt(random.Next(0, 0)).Value;
                Pack fled = new Pack(fledEnemies.Enemies.Count);
                foreach(Enemy e in fledEnemies.Enemies)
                {
                    fled.Add(e);
                }
                if (neighbourExit.AddPack(fled))
                {
                    //yay
                }
                else 
                {
                    fledEnemies = new Pack(50);
                }
                
                if (inCombat)
                {
                    IItem loot = itemGenerator.GetItem(node.Multiplier);
                    if (loot != null)
                    {
                        loot.Location = GetRandomLocation(4);
                        items.Add(loot);
                    }
                }
                inCombat = false;

                if (node.PackList.Count > 0)
                {
                    enemies = node.PackList[0];
                    inCombat = true;
                    PlaceEnemies(enemies);
                }
            }
        }

        public char[][] GetUpdatedMap()
        {
            CreateBackground();
            foreach (ITile i in items)
                map[i.Location.Y][i.Location.X] = i.Glyph;

            foreach (Enemy e in enemies)
                if(e.Alive)
                    map[e.Location.Y][e.Location.X] = e.Glyph;
            map[player.Location.Y][player.Location.X] = player.Glyph;
            return map;
        }

        public Point GetRandomLocation(int distFromWall)
        {
            return new Point(random.Next(Width - distFromWall - 2) + 1 + distFromWall / 2,
                random.Next(Height - distFromWall - 2) + 1 + distFromWall / 2);
        }

        private void CreateBackground()
        {
            map = new char[Height][];
            map[0] = new char[Width];
            for (int j = 0; j < map[0].Length; j++)
                if ((exits & Exit.Top) != Exit.Top || j <= topExit - doorsize || j >= topExit + doorsize)
                    map[0][j] = '█';
                else
                    map[0][j] = '▒';
            for (int i = 1; i < map.Length - 1; i++)
            {
                map[i] = new char[Width];
                for (int j = 0; j < map[i].Length; j++)
                    if (j == 0)
                    {
                        if ((exits & Exit.Left) != Exit.Left || i <= leftExit - doorsize || i >= leftExit + doorsize)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else if (j == map[i].Length - 1)
                    {
                        if ((exits & Exit.Right) != Exit.Right || i <= rightExit - doorsize || i >= rightExit + doorsize)
                            map[i][j] = '█';
                        else
                            map[i][j] = '▒';
                    }
                    else
                        map[i][j] = '.';

                map[map.Length - 1] = new char[Width];
                for (int j = 0; j < map[0].Length; j++)
                    if ((exits & Exit.Bot) != Exit.Bot || j <= botExit - doorsize || j >= botExit + doorsize)
                        map[map.Length - 1][j] = '█';
                    else
                        map[map.Length - 1][j] = '▒';
            }
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
            return HandleCombatRound(input);
        }

        public Player GetPlayer
        {
            get { return player; }
        }

        public void CreateDungeon(int packCount, int maxCapacity)
        {
            dungeonCreator = new DungeonCreator(random);
            dungeon = dungeonCreator.CreateDungeon(difficulty, packCount, maxCapacity);
            this.itemGenerator = new ItemGenerator(dungeon, player, random);
            InitRoom(dungeon.nodes[0]);
        }

        public string[] CreateEnemyOverview()
        {
            char[][] map = GetUpdatedMap();
            string[] drawMap = new string[map.Length];
            int i;
            drawMap[0] = new string(map[0]);
            for (i = 0; i < enemies.Size; i++ )
            {
                Enemy e = enemies[i];
                drawMap[i * 2 + 1] = string.Format("{0} {1}", new string(map[i * 2 + 1]), e.name.Substring(0, Math.Min(20, e.name.Length)));
                drawMap[i * 2 + 2] = string.Format("{0} {1} HP: {2}", new string(map[i * 2 + 2]), e.Glyph, e.CurrentHP);
            }
            for (i = i * 2 + 1; i < map.Length; i++)
                drawMap[i] = new string(map[i]);
            return drawMap;
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
            Console.Clear();
            if (CurrentNode.isBridge())
                Console.ForegroundColor = ConsoleColor.Red;
            else if (CurrentNode.IsExit)
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            else
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            string[] drawmap = CreateEnemyOverview();
            foreach (string s in drawmap)
                Console.WriteLine(s);
            Console.WriteLine(FormatHud());
        }

        public bool Save(string fileName)
        {
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || fileName == "")
            {
                Console.WriteLine("Your filename contains illegal characters. Press any Key to return.");
                if(acceptinput)
                    Console.ReadKey();
                return false;
            }

            if(File.Exists(fileName))
            {
                Console.WriteLine("There already is a save with that name. Do you want to overwrite? [y/n]");
                if(acceptinput)
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

        public bool Load(string fileName)
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
            return true;
        }

    }
}