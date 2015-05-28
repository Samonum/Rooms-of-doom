using System;

namespace RoomsOfDoom.Items
{
    public static class ItemGenerator
    {
        public static Random random;
        static Dungeon dungeon;
        static Player player;

        public static void Init(Random r, Dungeon d, Player p)
        {
            random = r;
            dungeon = d;
            player = p;
        }

        public static Loot GetItem(int multiplier)
        {
            double item = random.NextDouble();

            if (item < .05 * multiplier)
                return new Loot(1, '2');
            else if (item < .1 * multiplier)
                return new Loot(2, '3');
            
            int enemyHp = 0;
            
            foreach (Node n in dungeon.nodes)
                foreach (Pack p in n.PackList)
                    foreach (IHittable enemy in p.Enemies)
                        enemyHp += enemy.CurrentHP;

            if (enemyHp == 0)
                return null;
            if ((player.CurrentHP + player.GetPotCount * Potion.healPower) / enemyHp < item)
                return new Loot(0, '1');
            
            return null;
        }
    }
}
