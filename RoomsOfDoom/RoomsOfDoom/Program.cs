using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RoomsOfDoom
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "👹👹👹　Rooms of Dooooooooooooom　👹👹👹";
                Console.SetWindowSize(75, 31);
                Console.SetBufferSize(75, 31);
            }
            catch
            { }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.OutputEncoding = Encoding.UTF8;
            GameManager manager = new GameManager();
            while (true)
            {
                manager.Update();
            }
        }
    }
}
