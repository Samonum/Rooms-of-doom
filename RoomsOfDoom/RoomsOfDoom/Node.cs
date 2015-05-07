using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomsOfDoom
{
<<<<<<< HEAD
    public class Node
    {
        protected List<Node> adjacencyList;
        public int id;

        public Node(int id)
        {
            this.id = id;
            adjacencyList = new List<Node>();
        }

        public List<Node> AdjacencyList
        {
            get { return adjacencyList; }
            set { adjacencyList = value; }
        }

        public virtual String ToString()
        {
            String s = "N" + id + "(";

            foreach(Node n in adjacencyList)
                s += n.id + ",";

            s += ")";

            return s;
=======

    class Node
    {
        List<Pack> pack;

        Node[] Neighbours;
        bool bridge;

        Node(Node[] Neighbours, bool bridge)
        {

>>>>>>> 6a154a6e5c2589262f635ccaf26587fff736bc4e
        }
    }
}
