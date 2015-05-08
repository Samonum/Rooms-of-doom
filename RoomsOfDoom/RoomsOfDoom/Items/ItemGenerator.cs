using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    public class ItemGenerator
    {
        Random r;
        Dungeon dungeon;
        public ItemGenerator(Dungeon dungeon, Player p, Random r)
        {
            r = new Random();
            this.dungeon = dungeon;
        }

        public IItem GetItem(Point leftTop, Point rightBot, int multiplier)
        {
            double item = r.NextDouble();

            if (item < .05 * multiplier)
                return new TimeCrystal();
            else if (item < .1 * multiplier)
                return new MagicScroll(r);
            int MaxHp = 0;
            foreach (Node n in dungeon.nodes)
                foreach (Pack p in n.PackList)
                    foreach (IHittable enemy in p.Enemies)
                        MaxHp += enemy.CurrentHP;
            return null;
        }
    }
}
