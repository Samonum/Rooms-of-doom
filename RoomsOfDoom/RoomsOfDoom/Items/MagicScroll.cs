using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    class MagicScroll : IItem
    {
        public MagicScroll()
        {
            Duration = 10;
        }

        public void Use(Player player, Dungeon dungeon)
        {
            player.Multiplier *= 2;
        }

        public void Finish(Player player)
        {
            player.Multiplier /= 2; ;
        }

        public int Duration
        {
            get;
            set;
        }

        public int Id
        {
            get { return 2; }
        }
    }
}
