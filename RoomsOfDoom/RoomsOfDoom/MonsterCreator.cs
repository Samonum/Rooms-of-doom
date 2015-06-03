using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RoomsOfDoom
{
    public class MonsterCreator
    {
        private const int minDamage = 1, maxDamage = 10;
        private const int minSpeed = 3, maxSpeed = 8;
        public int maximumPackSize;
        Random r;

        public MonsterCreator(Random r, int maximumPackSize)
        {
            if (maximumPackSize <= 0)//packsize cannot be smaller than 1
                this.maximumPackSize = 1;
            else
                this.maximumPackSize = maximumPackSize;
            this.r = r;
        }

        public Pack GeneratePack(int difficulty)
        {
            Pack P = new Pack(r.Next(1, maximumPackSize));
            for (int i = 0; i < P.Enemies.Capacity; i++ )
            {
                Enemy e = CreateMonster(difficulty);
                P.Add(e);
            }

            return P;
        }

        public Enemy CreateMonster(int difficulty)
        {
            if (difficulty <= 0)//diff cannot be negative
                difficulty = 1;
            else if(difficulty >= 10000)//difficulty cannot be extremely large(might exceed maxint when multiplied)
                difficulty = 10000;
            string[] name = GenerateName().Split();

            int damage = r.Next(minDamage, maxDamage + 1);
            int speed = r.Next(minSpeed, maxSpeed + 1);

            Enemy e = new Enemy(name[1] + " " + name[2],name[0][0], r.Next(5*difficulty, 20 * difficulty), damage, speed);
            return e;
            //unit testing revealed bug with inputting negative difficulties
            //unit testing revealed bug with inputting very large numbers

        }

        public string GenerateName()
        {
            string[] adjectives = new string[] {"Giant", "Smelly", "Tiny", "Powerful", "Shady","Evil","Funky", "Quick", "Partying","Hooded","Infernal","Mutant","Sparkling","Shiny","Teenage","Ninja","Sneaky","Magnificent","Hairy","Quantum","Mighty","Bearded","Magical","Arcane","Divine","Jolly","Royal","Sophisticated","Overpowered","Travelling","Wandering","Awkward","Confident","Well-Mannered","Strange","Exotic"};
            Dictionary<string,string> names = new Dictionary<string,string>();
            //TODO this is not efficient
            using (StreamReader reader = new StreamReader("Enemies and packs/Names.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splitLine = line.Split('|');
                    names.Add(splitLine[1], splitLine[0]);
                }
            }

            KeyValuePair<string, string> randomKVP = names.ElementAt(r.Next(0, names.Count));
            string randomName = randomKVP.Key + " " + adjectives[r.Next(0, adjectives.Length)] + " " + randomKVP.Value;           
            return randomName;
        }

    }
}
