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
        private Dungeon dungeon;
        private Arena arena;
        private Random random;
        private Pack[] allEnemies;
        private MonsterCreator creator;
        private Player player;
        private int score = 0;
        //Healing Potions, Time Crystals, Magic Scrolls
        private byte[] inventory = new byte[3] { 0, 0, 0 };

        public GameManager(int seed = -1)
        {

            if (seed == -1)
                random = new Random();
            else
                random = new Random(seed);
            
            creator = new MonsterCreator(random, 6);
            allEnemies = new Pack[1] { creator.GeneratePack(1)};
            player = new Player();
            arena = new Arena(Exit.Left | Exit.Bot, allEnemies[0], player, Exit.Left, random);
        }

        public void Update()
        {
            Draw();
            HandleInput();

        }

        public void HandleInput()
        {
            char input = Console.ReadKey().KeyChar;
            arena.HandleCombatRound(input);
        }

        public void CreateDungeon(int size, int packs, int difficulty, int maxCapacity)
        {

        }


        public void IncreaseScore(int i)
        {
            if (i < 0)
                throw new Exception("Parameter may not be < 0");

            score += i;

            //Score wrapped to int.MinValue
            if (score < i)
                score = int.MaxValue;
        }

        public void AddPotion()
        {
            inventory[0]++;
        }

        public void AddCrystal()
        {
            inventory[1]++;
        }

        public void AddScroll()
        {
            inventory[2]++;
        }

        public int GetScore
        {
            get { return score; }
        }

        public Player GetPlayer
        {
            get { return player; }
        }

        public int GetPotCount
        {
            get { return inventory[0]; }
        }

        public int GetCrystalCount
        {
            get { return inventory[1]; }
        }

        public int GetScrollCount
        {
            get { return inventory[2]; }
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

new String[] { player.CurrentHP.ToString().PadLeft(4), score.ToString().PadLeft(14), 
    inventory[0].ToString().PadLeft(3), inventory[1].ToString().PadLeft(3), inventory[2].ToString().PadLeft(3) });
        }

        public void Draw()
        {
            Console.Clear();
            string[] drawmap = CreateEnemyOverview();
            foreach (string s in drawmap)
                Console.WriteLine(s);
            Console.WriteLine(FormatHud());
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter("save.txt"))
            {
                writer.Write(
                    GetPlayer.CurrentHP + ";" +
                    GetScore + ";" +
                    GetPotCount + ";" +
                    GetCrystalCount + ";" +
                    GetScrollCount
                );
            }
        }

        public void Load()
        {
            using (StreamReader reader = new StreamReader("save.txt"))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    string[] data = line.Split(';');
                    score = int.Parse(data[0]);
                    player = new Player(int.Parse(data[1]));
                    inventory = new byte[3]
                    {
                        byte.Parse(data[2]),
                        byte.Parse(data[3]),
                        byte.Parse(data[4])
                    };
                }
            }
        }
    }
}
