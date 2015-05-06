using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Enemy : IHittable
    {

        public string name;
        protected int maxHP;
        protected int currentHP;
        protected bool alive;


        public Enemy(string name,int hp)
        {
            this.name = name;
            myPack = null;
            maxHP = hp;
            currentHP = hp;
            alive = true;
        }

        public int Hit(int damage)
        {
            CurrentHP -= damage;
            return CurrentHP;
        }

        public Pack myPack
        {
            get;
            set;
        }

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
                    alive = false;
                if (currentHP > MaxHP)
                    currentHP = MaxHP;
            }
        }

        public int MaxHP
        {
            get{return maxHP;}
            set { maxHP = value; }
        }

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
    }
}
