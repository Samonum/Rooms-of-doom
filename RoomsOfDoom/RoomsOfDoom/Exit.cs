using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    [Flags]
    public enum Exit : byte
    {
        Top = 1,
        Bot = 2,
        Left = 4,
        Right = 8
    }
}
