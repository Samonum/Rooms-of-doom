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

        public GameManager(int seed = -1)
        {
            if (seed == -1)
                random = new Random();
            else
                random = new Random(seed);
        }

        public void CreateDungeon(int size, int packs, int difficulty, int maxCapacity)
        {

        }


    }
}
