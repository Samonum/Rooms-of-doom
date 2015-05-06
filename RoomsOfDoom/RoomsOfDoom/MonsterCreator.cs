using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    public class MonsterCreator
    {
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
            Enemy e = CreateMonster(difficulty);
            for (int i = 0; i < P.Enemies.Capacity; i++ )
            {
                P.Add(e);
            }

            return P;
        }

        public Enemy CreateMonster(int difficulty)
        {
            if (difficulty <= 0)//diff cannot be negative
                difficulty = 1;
            else if(difficulty >= 1000000)//difficulty cannot be extremely large(might exceed maxint when multiplied)
                difficulty = 1000000;
            Enemy e = new Enemy(GenerateName(), r.Next(5*difficulty, 10 * difficulty));
            
            return e;
            //unit testing revealed bug with inputting negative difficulties
            //unit testing revealed bug with inputting very large numbers

        }

        public string GenerateName()
        {
            string[] adjectives = new string[] {"Giant", "Smelly", "Tiny", "Powerful", "Shady","Evil","Funky", "Fast", "Partying","Hooded","Infernal","Mutant","Sparkling","Shiny"};
            string[] names = new string[] {"Goblin", "Wolf", "Orc","Dwarf","Elf","Bat","Bug", "NullreferenceException","Pony","Alien","Robot","Slime","Imp","Centipede" };            
            string createName = adjectives[r.Next(0,adjectives.Length)] + " " + names[r.Next(0,names.Length)];
            return createName;
        }

    }
}
