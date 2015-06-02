using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomsOfDoom
{
    public class Order
    {
        public static Order HuntOrder = new Order(null);

        public Order(Node targetNode)
        {
            Target = targetNode;
        }

        public Node Target
        {
            get;
            set;
        }
    }
}
