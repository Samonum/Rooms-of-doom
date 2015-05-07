using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Player : IHittable, ITile
    {

        public Player()
        {
            MaxHP = 100;
            currentHP = MaxHP;
            Alive = true;
        }
        
        public int Hit(int damage)
        {
            CurrentHP -= damage;
            return CurrentHP;
        }

        public bool Alive
        {
            get;
            private set;
        }

        int currentHP;
        public int CurrentHP
        {
            get
            {
                return currentHP;
            }
            set
            {
                currentHP = value;
                if (currentHP <= 0)
                    Alive = false;
                if (currentHP > MaxHP)
                    currentHP = MaxHP;
            }
        }

        public int MaxHP
        {
            get;
            private set;
        }

        public char Glyph
        {
            get { return '☻'; }
        }

        public Point Location
        {
            get;
            set;
        }
    }
}
