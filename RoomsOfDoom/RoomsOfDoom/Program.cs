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
            while (true)
            {
                stop.Restart();
                StringBuilder s = new StringBuilder(60 * 25);
                       
                for (int j = 0; j < 23; j++)
                {
                    for (int i = 0; i < 39; i++)
                        s.Append(Char.ConvertFromUtf32(/*50 + rand.Next(150)/*/ (i & 1) == 1 ? 'l' : 0x3042 + rand.Next(40)));
                    s.Append('\n');
                }
                Console.Write(stop.ElapsedMilliseconds);
                Thread.Sleep(1000 - (int)stop.ElapsedMilliseconds);
                Console.WriteLine(stop.ElapsedMilliseconds);
                Console.Clear();
                Console.WriteLine(stop.ElapsedMilliseconds);
                Console.Write(s.ToString());
                Console.WriteLine(stop.ElapsedMilliseconds);
            }
        }
    }
}
