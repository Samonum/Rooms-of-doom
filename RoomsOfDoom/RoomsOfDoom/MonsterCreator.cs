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
            else if(difficulty >= 10000)//difficulty cannot be extremely large(might exceed maxint when multiplied)
                difficulty = 10000;
            Enemy e = new Enemy(GenerateName(), r.Next(5*difficulty, 20 * difficulty));
            
            return e;
            //unit testing revealed bug with inputting negative difficulties
            //unit testing revealed bug with inputting very large numbers

        }

        public string GenerateName()
        {
            string[] adjectives = new string[] {"Giant", "Smelly", "Tiny", "Powerful", "Shady","Evil","Funky", "Quick", "Partying","Hooded","Infernal","Mutant","Sparkling","Shiny","Teenage","Ninja","Sneaky","Magnificent","Hairy","Quantum","Mighty","Bearded","Magical","Arcane","Divine","Jolly","Royal","Sophisticated","Overpowered","Travelling","Wandering","Awkward","Confident","Well-Mannered","Strange","Exotic"};
            string[] names = new string[] {"Goblin", "Wolf", "Orc","Dwarf","Elf","Bat","Bug", "NullreferenceException","Pony","Alien","Robot","Slime","Imp","Centipede","Turtle","Pirate","Dolphin","Wizard","Dragon","Programmer","Hexagon","Android","T-Rex","Sphinx","Bandit","Cultist","Necromancer","Spider","Salesman","Monster", "Pidgeon","Ooze","Skeleton","Zombie","Vampire","Werewolf","Witch","Shaman","Construct","Gnome","Kobold","Beholder" };            
            string createName = adjectives[r.Next(0,adjectives.Length)] + " " + names[r.Next(0,names.Length)];
            return createName;
        }

    }
}
