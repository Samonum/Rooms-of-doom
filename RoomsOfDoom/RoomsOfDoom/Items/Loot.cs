using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RoomsOfDoom.Items
{
    public class Loot : ITile
    {
        public Loot(int id, char glyph)
        {
            ID = id;
            Glyph = glyph;
        }

        public char Glyph
        {
            get;
            private set;
        }

        public int ID
        {
            get;
            private set;
        }

        public Point Location
        {
            get;
            set;
        }
    }
}
