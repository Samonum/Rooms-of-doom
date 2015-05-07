using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    abstract class Monster : IHittable
    {
        private int currentHP;

        public Monster(int hp)
        {
            currentHP = hp;
        }

        protected abstract void Die();

        public override int CurrentHP
        {
            get { return currentHP; }
            set 
            {
                currentHP = value;
                if (currentHP <= 0)
                    Die();
            }
        }
    }
}
