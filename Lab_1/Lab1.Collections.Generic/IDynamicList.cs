using System.Collections;
using System.Collections.Generic;

namespace Lab1.Collections.Generic
{
    public interface IDynamicList<T> : IEnumerable<T>, ICollection
    {
        int Capacity { get; set; }

        T this[int index] { get; set; }


        void Add(T item);

        bool Remove(T item);

        void RemoveAt(int index);

        void Clear();
    }
}