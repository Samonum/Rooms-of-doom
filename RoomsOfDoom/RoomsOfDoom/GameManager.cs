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
        private Random random;
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

            player = new Player();
        }

        public void HandleInput(Arena a)
        {
            char k = Console.ReadKey().KeyChar;
            switch(k)
            {
                case 'w': player.Move(Direction.Up, a.enemies);
                    break;
                case 'a': player.Move(Direction.Left, a.enemies);
                    break;
                case 's': player.Move(Direction.Down, a.enemies);
                    break;
                case 'd': player.Move(Direction.Right, a.enemies);
                    break;
                case '1'://usepotion()
                    break;
                case '2'://useScroll
                    break;
                case '3'://useCrystal
                    break;
            }

        }

        public void CreateDungeon(int size, int packs, int difficulty, int maxCapacity)
        {

        }

        public void IncreaseScore(int i)
        {
            score += i;
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

        public void DrawHud()
        {
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
