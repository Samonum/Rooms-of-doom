using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    class GameManager
    {
        Dungeon dungeon;
        Random random;
        Player player;

        public GameManager(int seed = -1)
        {
            if (seed == -1)
                random = new Random();
            else
                random = new Random(seed);

            player = new Player();
        }

        public void CreateDungeon(int size, int packs, int difficulty, int maxCapacity)
        {

        }

        public void DrawHud()
        {
            Console.WriteLine(String.Format(
@" ______________________________________
/                                      \\
| HP: {0}      POINTS: {1}   |
| 
|"),
 
new String[] {player.CurrentHP.ToString().PadLeft(4), "1".PadLeft(12)});
        }
    }
}
