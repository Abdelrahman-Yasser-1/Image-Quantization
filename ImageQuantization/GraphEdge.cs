using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    internal class GraphEdge
    {
        public int destination;//O(1) //parent --> destination
        public int source;//O(1) //current --> source
        public double weight;//O(1)

        public GraphEdge()
        {
        }

        public GraphEdge(int source, int destination, double weight)
        {
            this.source = source;
            this.destination = destination;
            this.weight = weight;
        }
    }
}
