﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace RoomsOfDoom
{
    class Log
    {
        GameManager manager;
        string[] replay;
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
            i = 0;
            n = 1;
            if (Finished())
                return;
            input = manager.acceptinput;
            manager.acceptinput = false;
            manager.Initialize(new DebugableRandom(int.Parse(replay[0].Trim())), false);
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

        int i, n;
        public void PlayStep()
        {
            if(i == 0)
                replay[n] = replay[n].Trim();

            if ((n & 1) == 0)
            {
                manager.random = new DebugableRandom(int.Parse(replay[n]));
                manager.difficulty--;
                manager.StartNextLevel();
                n++;
            }

            manager.Update(replay[n][i]);
            i++;
                
            if(i == replay[n].Length)
            {
                n++;
                i = 0;
            }
        }

        public bool Finished()
        {
            return replay == null || n >= replay.Length;
        }
    }
}
