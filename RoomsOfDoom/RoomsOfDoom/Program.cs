using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace RoomsOfDoom
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Random rand = new Random();
            GameManager manager = new GameManager();
            MonsterCreator M = new MonsterCreator(rand, 10);
            DungeonCreator D = new DungeonCreator(rand);
            Dungeon dungeon = D.GenerateDungeon(97, 4);


            Pack P = M.GeneratePack(1);
            Arena a = new Arena(Exit.Bot | Exit.Right | Exit.Left | Exit.Top, P, manager.GetPlayer, Exit.Top, rand);
            while (true)
            {
                manager.Update();
            }
        }
    }
}
