using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomsOfDoom
{
    public class Order
    {
        Node targetNode;

        public Order(Node targetNode)
        {
            this.targetNode = targetNode;
        }

        public Node Target
        {
            get { return targetNode; }
        }
    }
}
