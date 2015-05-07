using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace RoomsOfDoom
{
    public class InputHandler
    {

        public InputHandler()
        {

        }

        public void HandleInput()
        {
            ConsoleKeyInfo k = Console.ReadKey();
            Random r = new Random();

            switch(k.KeyChar)
            {
                case 'w': Console.Beep();
                     break;
                case 'a': Console.Beep(r.Next(37, 1000),50);//plz never use this
                    break;
                case 's': Console.Beep(r.Next(1000, 1004), 200);
                    break;
                case 'd': Console.Beep(r.Next(1000, 1004), 50);
                    break;
                case '1': Console.Beep(r.Next(800, 1200), 50);
                    break;
                case '2': Console.Beep(r.Next(800, 1200), 200);
                    break;
                case '3': Console.Beep(r.Next(300, 1500), 100);
                    break;
                     
            }
        }

        
        



    }
}
