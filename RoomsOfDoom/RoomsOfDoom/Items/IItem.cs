using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public interface IItem
    {
        void Use(Player player, Dungeon dungeon);

        void Finish(Player player);

        int Duration
        {
            get;
            set;
        }

        int Id
        {
            get;
        }
    }
}
