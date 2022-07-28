using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class MinHeap
    {
        public HeapNode[] heapNodes;//O(1)
        public int size;//O(1)
        public int[] indexes;//O(1)

        public MinHeap(int DistinctColorsCount)//O(1)
        {
            heapNodes = new HeapNode[DistinctColorsCount];//O(1)
            indexes = new int[DistinctColorsCount];//O(1)
            size = 0;//O(1)
            heapNodes[0] = new HeapNode();//O(1)
            heapNodes[0].key = double.MinValue;//O(1)
        }

        public void Add(HeapNode heapNode)//O(log(n))
        {
            heapNodes[size] = new HeapNode();//O(1)
            heapNodes[size].vertex = heapNode.vertex;//O(1)
            heapNodes[size].key = heapNode.key;//O(1)
            indexes[heapNode.vertex] = size;//O(1)
            Up(size);//O(log(n))
            size++;//O(1)
            
            //Total Complexity: O(log(n))
        }

        public void Up(int index)//O(log(n))
        {
            while (index > 0 && heapNodes[index].key < heapNodes[index / 2].key)//O(log(n))
            {
                //Swap child with parent
                HeapNode temp = heapNodes[index];//O(1)
                heapNodes[index] = heapNodes[index / 2];//O(1)
                heapNodes[index / 2] = temp;//O(1)
                //Swap indexes of child and parent
                indexes[heapNodes[index].vertex] = index;//O(1)
                indexes[heapNodes[index / 2].vertex] = index / 2;//O(1)
                index = index / 2;//O(1)
            }

            //Total Complexity: O(log(n))
        }

        public HeapNode GetRoot()//O(log(n))
        {
            HeapNode min = heapNodes[0];//O(1)
            heapNodes[0] = heapNodes[size - 1];//O(1)
            size--;//O(1)
            Down(0);//O(log(n))
            return min;//O(1)

            //Total Complexity: O(log(n))
        }

        public void Down(int index)//O(log(n))
        {
            int minIndex = index;//O(1)
            int left = 2 * index;//O(1)
            int right = 2 * index + 1;//O(1)
            if (left <= size && heapNodes[left].key < heapNodes[minIndex].key)//O(1)
                minIndex = left;//O(1)
            if (right <= size && heapNodes[right].key < heapNodes[minIndex].key)//O(1)
                minIndex = right;//O(1)
            if (minIndex != index)//O(1)
            {
                //Swap heapNodes
                HeapNode temp = heapNodes[index];//O(1)
                heapNodes[index] = heapNodes[minIndex];//O(1)
                heapNodes[minIndex] = temp;//O(1)
                //Swap indexes
                indexes[heapNodes[index].vertex] = index;//O(1)
                indexes[heapNodes[minIndex].vertex] = minIndex;//O(1)
                //repeat for the new index
                Down(minIndex);//O(log(n))
            }
            
            //Total Complexity: O(log(n))
        }

        public void ChangeKey(int vertex, double newKey)//O(log(n))
        {
            int index = indexes[vertex];//O(1)
            heapNodes[index].key = newKey;//O(1)
            Up(index);//O(log(n))
            
            //Total Complexity: O(log(n))
        }
    }
}
