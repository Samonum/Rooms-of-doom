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
        int MaxPackHP;

        public Pack(int packSize)
        {
            if (packSize <= 0)
                this.packSize = 1;
            else
                this.packSize = packSize;
            enemies = new List<Enemy>(this.packSize);
            //calculate maxPackHp
            foreach (Enemy e in enemies)
            {
                MaxPackHP += e.MaxHP;
            }
            order = null;
        }

        public void Add(Enemy enemy)
        {
            if (this.packSize == enemies.Count)
                return;
            this.enemies.Add(enemy);
            enemy.myPack = this;
        }

        public void GiveOrder(Order o)
        {
            order = o;
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return enemies.GetEnumerator();
        }
    }
}
