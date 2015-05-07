using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Player : IHittable, ITile
    {
        public const int strength = 10;
        //Healing Potions, Time Crystals, Magic Scrolls
        private byte[] inventory = new byte[3] { 2, 2, 2 };
        List<IItem> activeItems;

        public Player()
        {
            MaxHP = 100;
            currentHP = MaxHP;
            Alive = true;
            Multiplier = 1;
            activeItems = new List<IItem>();
        }
        
        public int Hit(int damage)
        {
            CurrentHP -= damage;
            return CurrentHP;
        }

        public bool Alive
        {
            get;
            private set;
        }

        int currentHP;
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
                    Alive = false;
                if (currentHP > MaxHP)
                    currentHP = MaxHP;
            }
        }

        public int MaxHP
        {
            get;
            private set;
        }

        public char Glyph
        {
            get { return '☻'; }
        }

        public Point Location
        {
            get;
            set;
        }

        public bool Move(Direction direction, Pack enemies)
        {
            Point loc = new Point();
            switch (direction)
            {
                case Direction.Up:
                    loc = new Point(Location.X, Location.Y - 1);
                    if (loc.Y == 0)
                        return false;
                    break;
                case Direction.Down:
                    loc = new Point(Location.X, Location.Y + 1);
                    if (loc.Y == Arena.Height - 1)
                        return false;
                    break;
                case Direction.Left:
                    loc = new Point(Location.X - 1, Location.Y);
                    if (loc.X == 0)
                        return false;
                    break;
                case Direction.Right:
                    loc = new Point(Location.X + 1, Location.Y);
                    if (loc.X == Arena.Width - 1)
                        return false;
                    break;
            }
            foreach(Enemy enemy in enemies)
                if(enemy.Location == loc)
                {
                    Combat(enemy);
                    return true;
                }
            Location = loc;
            return true;
        }

        public int Multiplier
        {
            get;
            set;
        }

        public bool OP
        {
            get;
            set;
        }

        public void Combat(Enemy enemy)
        {
            if(OP)
            {
                Enemy[] enemies = (Enemy[])enemy.myPack.Enemies.ToArray().Clone();
                foreach (Enemy e in enemies)
                    e.Hit(strength * Multiplier);
            }
            else
                enemy.Hit(strength * Multiplier);
        }

        public void UpdateItems()
        {
            IItem[] itemList = (IItem[])activeItems.ToArray().Clone();
            foreach (IItem i in itemList)
            {
                if (i.Duration == 0)
                {
                    activeItems.Remove(i);
                    i.Finish(this);
                }
                i.Duration--;
            }
        }


        public bool UseItem(IItem item, Dungeon dungeon)
        {
            if (inventory[item.Id] <= 0)
                return false;
            inventory[item.Id]--;
            item.Use(this, dungeon);
            activeItems.Add(item);
            return true;
        }

        public void AddPotion()
        {
            inventory[0]++;
        }

        public void AddCrystal()
        {
            inventory[1]++;
        }

        public void AddScroll()
        {
            inventory[2]++;
        }

        public int GetPotCount
        {
            get { return inventory[0]; }
        }

        public int GetCrystalCount
        {
            get { return inventory[1]; }
        }

        public int GetScrollCount
        {
            get { return inventory[2]; }
        }

    }
}
