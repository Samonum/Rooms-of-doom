using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomsOfDoom
{
    public class Gate
    {
        int location;
        int size;
        Node leadsTo;

        private const int minDoorSize = 3;
        private const int maxDoorSize = 3;

        public Gate(int location, int size, Node leadsTo)
        {
            this.location = location;
            this.size = size;
            this.leadsTo = leadsTo;
        }

        public int Location
        {
            get { return location; }
        }

        public int Size
        {
            get { return size; }
        }

        public Node LeadsTo
        {
            get { return leadsTo; }
        }
    }
}
