using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Magic
    {
        public double mana = 100;
        public Magic(double beginMana)
        {
            mana = beginMana;
        }
        
        public void Cast(double amount)
        {
            if (amount > mana)
            {
                throw new ArgumentOutOfRangeException("amount");
            }
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount");
            }
            mana -= amount;
        }

    }
}
