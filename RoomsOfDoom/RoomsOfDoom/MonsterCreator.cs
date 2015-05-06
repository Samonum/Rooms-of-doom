using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
    class MonsterCreator
    {
        int maximumPackSize;
        Random r;

        public MonsterCreator(Random r, int maximumPackSize)
        {
            this.maximumPackSize = maximumPackSize;
            this.r = r;
        }

        public Pack GeneratePack(int difficulty)
        {
            Pack P = new Pack(r.Next(0, maximumPackSize));
            Enemy e = CreateMonster(difficulty);
            for (int i = 0; i < P.Enemies.Capacity; i++ )
            {
                P.Add(e);
            }

            return P;
        }

        public Enemy CreateMonster(int difficulty)
        {
            Enemy e = new Enemy(GenerateName(), r.Next(5*difficulty, 50 * difficulty));
            return e;

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
