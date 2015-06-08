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

        public void EnterHighScore(int score, string name)
        {
            //Console.Clear();
            string formattedScore = name + "|" + score;

            using(StreamWriter writer = new StreamWriter("HighScores.txt",true))
            {
                writer.WriteLine(formattedScore);
            }

            Console.WriteLine("Congratulations: " + name + ", Your score had been submitted!" );
        }

        public Tuple<int, string>[] LoadScores()
        {
            List<Tuple<int, string>> scores = new List<Tuple<int, string>>();
            string line;
            //Console.Clear();
            using (StreamReader reader = new StreamReader("HighScores.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splitLine = line.Split('|');
                    scores.Add(new Tuple<int, string>(int.Parse(splitLine[1]), splitLine[0]));
                }
            }

            IOrderedEnumerable<Tuple<int, string>> sorted = from pair in scores
                                                            orderby pair.Item1 descending
                                                            select pair;

            return sorted.ToArray();
        }

        public void displayHighScores()
        {
            Tuple<int, string>[] scores = LoadScores();
                //scores = (Dictionary<int,string>)sorted;
                int counter = 1;
                foreach(Tuple<int,string> pair in scores)
                {
                    //KeyValuePair<int, string> pair = scores.First<KeyValuePair<int, string>>();
                    Console.WriteLine(counter + ": " + pair.Item2 + " " + pair.Item1.ToString());
                    counter++;
                }
        }

    }
}
