using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    public class Heap<TPriority, TValue> where TPriority : IComparable
    {

        protected struct HeapNode
        {
            public TValue value;
            public TPriority priority;
        }

        private List<HeapNode> heapTree;

        public Heap()
        {
            heapTree = new List<HeapNode>();
        }

        public void SetPair(TValue value, TPriority priority)
        {
            var newNode = new HeapNode();
            newNode.value = value;
            newNode.priority = priority;
            heapTree.Add(newNode);
                    
        }

     


    }
}
