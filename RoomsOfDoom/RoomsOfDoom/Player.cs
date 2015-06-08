using RoomsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Player : IHittable, ITile
    {
        public const int strength = 10;
        //Healing Potions, Time Crystals, Magic Scrolls
        public byte[] inventory = new byte[4] { 2, 2, 2, 0 };
        List<IItem> activeItems;
        private int score = 0;

        public Player(int curHp = -1)
        {
            MaxHP = 100;
            if (curHp == -1)
                currentHP = MaxHP;
            else if (curHp > MaxHP)
                currentHP = MaxHP;
            else
                currentHP = curHp;
            Alive = true;
            Multiplier = 1;
            activeItems = new List<IItem>();
            ScoreMultiplier = 1;
        }

        public bool Hit(int damage)
        {
            CurrentHP -= damage;
            return !Alive;
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

        public int ScoreMultiplier
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
                    if (loc.Y == GameManager.Height - 1)
                        return false;
                    break;
                case Direction.Left:
                    loc = new Point(Location.X - 1, Location.Y);
                    if (loc.X == 0)
                        return false;
                    break;
                case Direction.Right:
                    loc = new Point(Location.X + 1, Location.Y);
                    if (loc.X == GameManager.Width - 1)
                        return false;
                    break;
            }
            if (enemies != null)
                foreach (Enemy enemy in enemies)
                    if (enemy.Location == loc)
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
                Enemy[] pack = (Enemy[])enemy.myPack.Enemies.ToArray().Clone();
                foreach (Enemy packman in pack)
                    if (packman.Hit(strength * Multiplier))
                        IncreaseScore(packman.GetScore());
            }
            else
                if (enemy.Hit(strength * Multiplier))
                    IncreaseScore(enemy.GetScore());
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
            if (item.Id < 0 || item.Id > inventory.Length)
                return false;
            if (inventory[item.Id] <= 0)
                return false;
            inventory[item.Id]--;
            item.Use(this, dungeon);
            activeItems.Add(item);
            return true;
        }

        public void IncreaseScore(int i)
        {
            if (i < 0)
                i = 0;

            score += i * ScoreMultiplier;

            //Score wrapped to int.MinValue
            if (score < i)
                score = int.MaxValue;
        }

        public int GetScore
        {
            get { return score; }
        }

        public void SetItems(byte potion, byte crystal, byte scroll)
        {
            inventory[0] = potion;
            inventory[1] = crystal;
            inventory[2] = scroll;
        }

        public void AddItem(Loot loot)
        {
            if (loot.ID < 0 || loot.ID > inventory.Length)
                return;
            if (inventory[loot.ID] == 255)
                return;
            inventory[loot.ID]++;
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
