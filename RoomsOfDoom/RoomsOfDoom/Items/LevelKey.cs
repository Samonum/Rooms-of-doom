using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    class LevelKey : IItem
    {
        GameManager manager;
        public LevelKey(GameManager manager)
        {
            this.manager = manager;
        }

        public void Use(Player player, Dungeon dungeon)
        {
            manager.StartNextLevel();
        }

        public void Finish(Player player)
        {
        }

        public int Duration
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public int Id
        {
            get { return 3; }
        }

        public char Glyph
        {
            get { return '>'; }
        }

        public System.Drawing.Point Location
        {
            get;
            set;
        }
    }
}
