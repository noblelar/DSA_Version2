using System;
using System.Collections.Generic;
using DAS_Coursework.models;

namespace DAS_Coursework.utils
{
    public class PriorityQueue
    {
        private List<(Verticex, double)> queue;

        public PriorityQueue()
        {
            queue = new List<(Verticex, double)>();
        }

        public void Enqueue(Verticex vertex, double priority)
        {
            queue.Add((vertex, priority));
            Sort();
        }

        public Verticex Dequeue()
        {
            if (IsEmpty()) return null;
            Verticex vertex = queue[0].Item1;
            queue.RemoveAt(0);
            return vertex;
        }

        public bool IsEmpty()
        {
            return queue.Count == 0;
        }

        private void Sort()
        {
            queue.Sort(new PriorityQueueComparer());
        }

        private class PriorityQueueComparer : IComparer<(Verticex, double)>
        {
            public int Compare((Verticex, double) x, (Verticex, double) y)
            {
                return x.Item2.CompareTo(y.Item2);
            }
        }
    }
}
