using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Pack
    {
        int packSize;
        List<Enemy> enemies;


        public Pack(int packSize)
        {
            if (this.packSize <= 0)
                packSize = 1;
            this.packSize = packSize;
            enemies = new List<Enemy>(packSize);
        }

        public void Add(Enemy enemy)
        {
            if (enemies.Capacity == enemies.Count)
                return;
            this.enemies.Add(enemy);
            enemy.myPack = this;
        }

        public List<Enemy> Enemies
        {
            get{return this.enemies;}
            set { this.enemies = value; }
        }

    }
}
