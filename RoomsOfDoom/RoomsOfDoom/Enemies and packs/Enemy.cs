using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Enemy : IHittable, ITile
    {
        
        public string name;
        protected int maxHP;
        protected int currentHP;
        protected bool alive;
        protected char glyph;
        public int damage;
        public int speed;
        protected int moveCounter;

        public Enemy(string name, char glyph, int hp, int damage = MonsterCreator.minDamage, int speed = MonsterCreator.maxDamage)
        {
            this.name = name;
            myPack = null;
            maxHP = hp;
            currentHP = hp;
            alive = true;
            this.glyph = glyph;
            this.damage = damage;
            this.speed = speed;
            moveCounter = 0;
        }

        public bool Hit(int damage)
        {
            CurrentHP -= damage;
            return !Alive;
        }

        public bool CanMove()
        {
            if (moveCounter >= speed)
            {
                moveCounter = 0;
                return false;
            }

            moveCounter++;
            return true;
        }

        public int GetScore()
        {
            double damageMultiplier = (double)(damage - MonsterCreator.minDamage) / (double)(MonsterCreator.maxDamage - MonsterCreator.minDamage);
            double speedMultiplier = (double)(speed - MonsterCreator.minSpeed) / (double)(MonsterCreator.maxSpeed - MonsterCreator.minSpeed);
            double totalMultiplier = (1.0 + (damageMultiplier + speedMultiplier) / 2.0);
            return (int)(name.Length * totalMultiplier);
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
                {
                    myPack.Enemies.Remove(this);
                    alive = false;
                }
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

        public char Glyph
        {
            get { return glyph; }
        }

        public Point Location
        {
        get;
        set;
        }

        public void KillTheHeretic(IHittable p)
        {
            p.Hit(damage);
        }

        public String GetStats(bool debug = false)
        {
            if (debug)
                return string.Format("{0} HP: {1} Spd: {2} Dmg: {3}", new string[] { Glyph.ToString(), CurrentHP.ToString(), speed.ToString(), damage.ToString() });
            return string.Format("{0} HP: {1}", new string[] { Glyph.ToString(), CurrentHP.ToString() });
        }
    }
}
