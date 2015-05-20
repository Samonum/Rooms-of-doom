using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RoomsOfDoom
{
    public class HighScores
    {

        public HighScores()
        {

        }

        public void EnterHighScore(int score)
        {
            //Console.Clear();
            Console.WriteLine("Your score is: " + score + " ! Please enter your name:");
            string name = Console.ReadLine();
            string formattedScore = name + "|" + score;

            using(StreamWriter writer = new StreamWriter("HighScores.txt",true))
            {
                writer.WriteLine(formattedScore);
            }

            Console.WriteLine("Congratulations: " + name + ", Your score had been submitted!" );
        }

        public void displayHighScores()
        {
            //Console.Clear();
            using (StreamReader reader = new StreamReader("HighScores.txt"))
            {
                List<Tuple<int, string>> scores = new List<Tuple<int, string>>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splitLine = line.Split('|');
                    scores.Add(new Tuple<int,string>(int.Parse(splitLine[1]), splitLine[0]));
                }
                var sorted = from pair in scores
                             orderby pair.Item1 descending
                             select pair;
                //scores = (Dictionary<int,string>)sorted;
                int counter = 1;
                foreach(Tuple<int,string> pair in sorted)
                {
                    //KeyValuePair<int, string> pair = scores.First<KeyValuePair<int, string>>();
                    Console.WriteLine(counter + ": " + pair.Item2 + " " + pair.Item1.ToString());
                    scores.Remove(pair);
                    counter++;
                }
            }

        }

    }
}
