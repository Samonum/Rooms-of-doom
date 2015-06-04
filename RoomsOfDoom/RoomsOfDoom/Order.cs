using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomsOfDoom
{
    public class Order
    {
        public static Order HuntOrder = new Order(null, "▒");

        public Order(Node targetNode, string glyph = "█")
        {
            Glyph = glyph;
            Target = targetNode;
        }

        public Node Target
        {
            get;
            set;
        }

        public string Glyph
        {
            get;
            private set;
        }

        public string ToString()
        {
            string s = "";

            if (Target != null)
                s += Target.id + Glyph;
            else
                s += -1 + Glyph;

            return s;
        }
    }
}
