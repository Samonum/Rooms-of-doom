using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Pack : IEnumerable<Enemy>
    {
        int packSize;
        List<Enemy> enemies;
        private Order order;
        protected int maxPackHP;

        public Pack(int packSize)
        {
            if (packSize <= 0)
                this.packSize = 1;
            else
                this.packSize = packSize;
            enemies = new List<Enemy>(this.packSize);
            order = null;
        }

        public void Add(Enemy enemy)
        {
            if (this.packSize == enemies.Count)
                return;
            this.enemies.Add(enemy);
            enemy.myPack = this;
            this.maxPackHP += enemy.MaxHP;
        }

        public bool GiveOrder(Order o)
        {
            if (order != null)
                return false;
            order = o;
            return true;
        }

        public Node Target
        {
            get 
            {
                if (order == null)
                    return null;

                return order.Target; 
            }
        }

        public List<Enemy> Enemies
        {
            get{return this.enemies;}
        }

        public Enemy this[int index]
        {
            get { return enemies[index]; }
        }

        public IEnumerator<Enemy> GetEnumerator()
        {
            return enemies.GetEnumerator();
        }
        public int Size
        {
            get { return enemies.Count; }
        }

        public int MaxPackHP
        {
            get { return maxPackHP; }
        }

        public int CurrentPackHP
        {
            get 
            {
                int countHP = 0;
                foreach (Enemy e in enemies)
                {
                    countHP += e.CurrentHP;
                }
                return countHP;
            }
        }

        public override String ToString()
        {
            string s = "(";

            foreach (Enemy e in enemies)
                s += e.Glyph;

            s += ")";

            return s;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return enemies.GetEnumerator();
        }
    }
}
