using System.Drawing;

namespace RoomsOfDoom
{
    public interface ITile
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
