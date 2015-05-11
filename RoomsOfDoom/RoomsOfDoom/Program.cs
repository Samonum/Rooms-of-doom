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
            Console.Title = "👹👹👹　Rooms of Dooooooooooooom　👹👹👹";
            Console.SetWindowSize(75, 31);
            Console.SetBufferSize(75, 31);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
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
