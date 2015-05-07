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
            while (true)
            {
                Arena a = new Arena(Exit.Bot, new Pack(1), manager.GetPlayer, Exit.Bot);
                a.UpdateMap();
                a.Draw();


                manager.IncreaseScore(rand.Next(100000));
                stop.Restart();
                manager.DrawHud();
                StringBuilder s = new StringBuilder(60 * 25);

                //code to show pack creation works
                MonsterCreator M = new MonsterCreator(rand, 10);
                Pack P = M.GeneratePack(1);
                foreach (Enemy e in P.Enemies)
                {
                    Console.WriteLine("Generated: " + e.name + " with " + e.CurrentHP + " HP!");
                }
                Thread.Sleep(Math.Max(0, 1000 - (int)stop.ElapsedMilliseconds));
                Console.Clear();
            }
        }
    }
}
