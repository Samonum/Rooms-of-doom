using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    public class Potion : IItem
    {
        public const int healPower = 10;
        public Potion()
        {

        }

        public void Use(Player player, Dungeon dungeon)
        {
            player.CurrentHP += healPower;
        }
        public void Finish(Player player)
        {
        }

        public int Duration
        {
            get { return 0; }
            set { }
        }

        public int Id
        {
            get { return 0; }
        }

        public System.Drawing.Point Location
        {
            get;
            set;
        }

        public char Glyph
        {
            get { return '1'; }
        }
    }
}
