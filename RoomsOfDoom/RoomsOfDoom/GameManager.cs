using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class GameManager
    {
        private DungeonCreator dungeonCreator;
        private Dungeon dungeon;
        private Arena arena;
        private Random random;
        private Player player;
        private int difficulty;

        public GameManager(int seed = -1)
        {
            if (seed == -1)
                random = new Random();
            else
                random = new Random(seed);

            difficulty = 1;

            dungeonCreator = new DungeonCreator(random);
            CreateDungeon(difficulty, 10, 10);
            player = new Player();
            arena = new Arena(dungeon.nodes[0], player, random, new Items.ItemGenerator(dungeon, player, random));
            
        }

        public void Update()
        {
            Draw();
            HandleInput();
            player.UpdateItems();
        }

        public void HandleInput()
        {
            char input = Console.ReadKey().KeyChar;
            switch (input)
            {
                case 'o':
                    Console.WriteLine("How would you like to Call your Save?");
                    Save(Console.ReadLine());
                    break;
                case 'l':
                    Console.WriteLine("What savefile would you like to load?");
                    Load(Console.ReadLine());
                    break;
                default:
                    arena.HandleCombatRound(input);
                    break;
            }
            dungeon.Update();
        }

        public Player GetPlayer
        {
            get { return player; }
        }

        public void CreateDungeon(int difficulty, int packCount, int maxCapacity)
        {
            dungeon = dungeonCreator.CreateDungeon(difficulty, packCount, maxCapacity);
        }

        public string[] CreateEnemyOverview()
        {
            char[][] map = arena.GetUpdatedMap();
            string[] drawMap = new string[map.Length];
            int i;
            drawMap[0] = new string(map[0]);
            for (i = 0; i < arena.enemies.Size; i++ )
            {
                Enemy e = arena.enemies[i];
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
| HP: {0}        POINTS: {1}         |
| POT: {2}        TC: {3}        MS: {4}         |
\________________________________________________/ ",

new String[] { player.CurrentHP.ToString().PadLeft(4), player.GetScore.ToString().PadLeft(14), 
    player.GetPotCount.ToString().PadLeft(3), player.GetCrystalCount.ToString().PadLeft(3), player.GetScrollCount.ToString().PadLeft(3) });
        }

        public void Draw()
        {
            Console.Clear();
            if (arena.CurrentNode.isBridge())
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            string[] drawmap = CreateEnemyOverview();
            foreach (string s in drawmap)
                Console.WriteLine(s);
            Console.WriteLine(FormatHud());
        }

        public void Save(string fileName)
        {
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Console.WriteLine("Your filename contains illegal characters. Press any Key to return.");
                Console.ReadKey();
                return;
            }
            Player p = GetPlayer;
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(
                    p.CurrentHP + ";" +
                    p.GetScore + ";" +
                    p.GetPotCount + ";" +
                    p.GetCrystalCount + ";" +
                    p.GetScrollCount
                );
            }
        }

        public void Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File Does Not Exist. Press any Key to return.");
                Console.ReadKey();
                return;
            }
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    string[] data = line.Split(';');
                    player.IncreaseScore(int.Parse(data[0]));
                    player = new Player(int.Parse(data[1]));
                    player.SetItems(
                        byte.Parse(data[2]),
                        byte.Parse(data[3]),
                        byte.Parse(data[4])
                    );
                }
            }
        }
    }
}
