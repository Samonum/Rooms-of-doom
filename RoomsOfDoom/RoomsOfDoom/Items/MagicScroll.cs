using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    public class MagicScroll : IItem
    {
        public const double stability = .85;
        Random r;
        GameManager arena;
        bool undoable;
        public MagicScroll(Random r, GameManager arena)
        {
            Duration = 10;
            this.r = r;
            this.arena = arena;
        }

        public void Use(Player player, Dungeon dungeon)
        {
            if (r.NextDouble() > stability)
            {
                List<Node> n = dungeon.Destroy(arena.CurrentNode);
                if (n != null)
                    arena.InitRoom(n[0]);
            }
            else
            {
                player.Multiplier *= 2;
                undoable = true;
            }
        }

        public void Finish(Player player)
        {
            if(undoable)
                player.Multiplier /= 2;
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
