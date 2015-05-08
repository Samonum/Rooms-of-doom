using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    public class MagicScroll : IItem
    {
        public const double explosiveness = 0.15;
        Random r;
        Arena arena;
        public MagicScroll(Random r, Arena arena)
        {
            Duration = 10;
            this.r = r;
            this.arena = arena;
        }

        public void Use(Player player, Dungeon dungeon)
        {
            if (r.NextDouble() > explosiveness) 
                arena.InitRoom(dungeon.Destroy(arena.CurrentNode)[0]);
            else
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

        public System.Drawing.Point Location
        {
            get;
            set;
        }

        public char Glyph
        {
            get { return '2'; }
        }
    }
}
