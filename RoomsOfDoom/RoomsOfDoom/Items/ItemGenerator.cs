using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    public class ItemGenerator
    {
        Random r;
        Dungeon dungeon;
        public ItemGenerator(Dungeon dungeon, Random r)
        {
            r = new Random();
            dungeon = dungeon;
        }

        public IItem GetItem(Point leftTop, Point rightBot, int multiplier)
        {


            return null;
        }
    }
}
