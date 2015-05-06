using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public interface IHittable
    {
        bool Alive
        {
            get;
        }

        int CurrentHP
        {
            get;
            set;
        }

        int MaxHP
        {
            get;
        }

        int Hit(int damage);
    }
}
