using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom.Items
{
    public class LevelKey : IItem
    {
        GameManager manager;
        public LevelKey(GameManager manager)
        {
            this.manager = manager;
        }

        public void Use(Player player, Dungeon dungeon)
        {
            if (manager.CurrentNode.IsExit)
            {
                player.IncreaseScore(manager.difficulty * 100);
                manager.StartNextLevel();
            }
            else 
                player.AddItem(new Loot(3, '>'));
        }

        public void Finish(Player player)
        {
        }

        public int Duration
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public int Id
        {
            get { return 3; }
        }
    }
}
