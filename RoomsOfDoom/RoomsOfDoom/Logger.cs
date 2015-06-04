using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RoomsOfDoom
{
    public class Logger
    {
        StringBuilder log = new StringBuilder();

        public void WriteLine(string text)
        {
            Write(text);
            Write("\n");
        }

        public void WriteLine(int text)
        {
            WriteLine(text.ToString());
        }

        public void Write(string text)
        {
            log.Append(text);
        }

        public void Write(char text)
        {
            log.Append(text.ToString());
        }

        public void Save(string path, bool inPlay)
        {
            if (path == "")
                return;
            using(StreamWriter writer = new StreamWriter(path + (inPlay ? ".inplay" : ".play")))
                writer.Write(log);
        }

        public void Load(string path)
        {
            log = new StringBuilder();
            if (path == "")
                return;
            using (StreamReader reader = new StreamReader(path + ".inplay"))
                log.Append(reader.ReadToEnd());
        }


    }
}
