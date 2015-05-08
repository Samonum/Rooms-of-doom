using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    public class ItemGenerator
    {
        public Arena Arena
        {
            get;
            set;
        }
        Random random;
        Dungeon dungeon;
        Player player;
        public ItemGenerator(Dungeon dungeon, Player player, Random random)
        {
            this.random = random;
            this.dungeon = dungeon;
            this.player = player;
        }

        public IItem GetItem(int multiplier)
        {
            double item = random.NextDouble();

            if (item < .05 * multiplier)
                return new TimeCrystal();
            else if (item < .1 * multiplier)
                return new MagicScroll(random, Arena);
            int enemyHp = 0;

            foreach (Node n in dungeon.nodes)
                foreach (Pack p in n.PackList)
                    foreach (IHittable enemy in p.Enemies)
                        enemyHp += enemy.CurrentHP;

            if (enemyHp == 0)
                return null;
            if ((player.CurrentHP + player.GetPotCount * Potion.healPower) / enemyHp < item)
                return new Potion();
            return null;
        }
    }
}
