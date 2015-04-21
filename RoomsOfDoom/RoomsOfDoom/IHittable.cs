using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    interface IHittable
    {
        int CurrentHP
        {
            get;
            set;
        }

        int MaxHP
        {
            get;
        }
    }
}
