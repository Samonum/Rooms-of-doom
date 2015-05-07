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
            Stopwatch stop = new Stopwatch();
            Random rand = new Random();
            GameManager manager = new GameManager();
            MonsterCreator M = new MonsterCreator(rand, 10);
            DungeonCreator D = new DungeonCreator(rand);
            Dungeon dungeon = D.GenerateDungeon(97, 4);
            while (true)
            {
                stop.Restart();
                Pack P = M.GeneratePack(1);
                Arena a = new Arena(Exit.Bot | Exit.Right | Exit.Left | Exit.Top, P, manager.GetPlayer, Exit.Top, rand);
                a.UpdateMap();
                a.Draw();
                manager.IncreaseScore(rand.Next(100000));
                manager.DrawHud();
                //Thread.Sleep(Math.Max(0, 1000 - (int)stop.ElapsedMilliseconds));
                Console.Clear();
            }
        }
    }
}
