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
            while (true)
            {
                manager.Update();
            }
        }
    }
}
