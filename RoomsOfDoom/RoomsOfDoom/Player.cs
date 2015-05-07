using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class Player : IHittable, ITile
    {

        public Player()
        {
            MaxHP = 100;
            currentHP = MaxHP;
            Alive = true;
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
                    if (loc.X == Arena.Height - 1)
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

        public void Combat(Enemy enemy)
        {

        }

    }
}
