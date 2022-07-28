using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class HeapNode
    {
        public int vertex;//O(1)
        public double key;//O(1)

        public HeapNode()//O(1)
        { 

        }

        public HeapNode(int v, double k)//O(1)
        {
            vertex = v;//O(1)
            key = k;//O(1)
        }
    }
}
