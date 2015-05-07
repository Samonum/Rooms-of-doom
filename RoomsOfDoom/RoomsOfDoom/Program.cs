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
            DungeonCreator D = new DungeonCreator(rand);
            Dungeon dungeon = D.GenerateDungeon(1);
            MonsterCreator M = new MonsterCreator(rand, 10);
            Pack P = M.GeneratePack(1);
            Arena a = new Arena(Exit.Bot | Exit.Right | Exit.Left | Exit.Top, P, manager.GetPlayer, Exit.Top, rand);
            while (true)
            {
                stop.Restart();
                a.UpdateMap();
                a.Draw();
                manager.DrawHud();

                foreach (Enemy e in P.Enemies)
                {
                    Console.WriteLine("Generated: " + e.name + " with " + e.CurrentHP + " HP!");
                }

                Console.WriteLine(dungeon.ToString());
                
                //Thread.Sleep(Math.Max(0, 1000 - (int)stop.ElapsedMilliseconds));
              
                manager.HandleInput(a);

                Console.Clear();
            }
        }
    }
}
