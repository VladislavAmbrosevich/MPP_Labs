using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Collections.Generic
{
    public interface IDynamicList<T> : IEnumerable<T>
    {
        int Count { get; }

        int Capacity { get; set; }

        T this[int index] { get; set; }


        void Add(T item);

        bool Remove(T item);

        void RemoveAt(int index);

        void Clear();
    }
}