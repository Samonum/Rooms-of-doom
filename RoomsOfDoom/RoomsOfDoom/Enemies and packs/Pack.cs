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


        public Pack(int packSize)
        {
            if (packSize <= 0)
                this.packSize = 1;
            else
                this.packSize = packSize;
            enemies = new List<Enemy>(this.packSize);
        }

        public void Add(Enemy enemy)
        {
            if (this.packSize == enemies.Count)
                return;
            this.enemies.Add(enemy);
            enemy.myPack = this;
        }

        public List<Enemy> Enemies
        {
            get{return this.enemies;}
        }

        Enemy this[int index]
        {
            get { return enemies[index]; }
        }

        public IEnumerator<Enemy> GetEnumerator()
        {
            return enemies.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return enemies.GetEnumerator();
        }
    }
}
