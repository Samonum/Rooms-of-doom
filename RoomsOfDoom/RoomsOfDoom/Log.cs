using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace RoomsOfDoom
{
    public class Log
    {
        GameManager manager;
        public string[] replay;
        bool input;


        public Log(GameManager manager)
        {
            this.manager = manager;
        }

        public Log(GameManager manager, string path) : this(manager)
        {
            LoadReplay(path);
        }

        public bool LoadReplay(string s)
        {
            s += ".play";
            if (!File.Exists(s))
            {
                Console.WriteLine("File Does Not Exist. Press any Key to return.");
                if (manager.acceptinput)
                    Console.ReadKey();
                return false;
            }

            using (StreamReader reader = new StreamReader(s))
            {
                replay = reader.ReadToEnd().Split('\n');
            }
            return true;
        }
        
        public void Initialize()
        {
            input = manager.acceptinput;
            manager.acceptinput = false;
            if (Finished())
                return;
            readIndex = 0;
            readLineNumber = 1;
            manager.Initialize(new DebugableRandom(int.Parse(replay[0].Trim())));
        }

        public void CleanUp()
        {
            manager.acceptinput = input;
        }

        public void PlayReplay(int speed)
        {
            Initialize();
            while (!Finished())
            {
                Thread.Sleep(speed);
                PlayStep();
            }
            CleanUp();
        }

        int readIndex, readLineNumber;
        public void PlayStep()
        {
            if (Finished())
                return;

            if(readIndex == 0)
                replay[readLineNumber] = replay[readLineNumber].Trim();

            if ((readLineNumber & 1) == 0)
            {
                manager.random = new DebugableRandom(int.Parse(replay[readLineNumber]));
                manager.difficulty--;
                manager.StartNextLevel();
                readLineNumber++;
                replay[readLineNumber] = replay[readLineNumber].Trim();
            }

            manager.Update(replay[readLineNumber][readIndex]);

            readIndex++;

            if(readIndex == replay[readLineNumber].Length)
            {
                readLineNumber++;
                readIndex = 0;
            }
        }

        public bool Finished()
        {
            return replay == null || readLineNumber >= replay.Length;
        }
    }
}
