using System.Drawing;

namespace RoomsOfDoom
{
    interface ITile
    {
        char Glyph
        {
            get;
        }

        Point Location
        {
            get;
            set;
        }
    }
}
