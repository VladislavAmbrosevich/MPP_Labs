using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Lab1.Collections.Generic
{
    public class DynamicList<T> : IDynamicList<T>
    {
        private static readonly T[] EmptyArray = new T[0];

        private const int DefaultCapacity = 4;

        private T[] _items;

        [NonSerialized]
        private object _syncRoot;


        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                }

                return _syncRoot;
            }
        }

        bool ICollection.IsSynchronized => false;


        public int Count { get; private set; }

        public int Capacity
        {
            get => _items.Length;
            set
            {
                if (value < Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                if (value == _items.Length)
                {
                    return;
                }
                if (value > 0)
                {
                    var objArray = new T[value];
                    if (Count > 0)
                    {
                        Array.Copy(_items, 0, objArray, 0, Count);
                    }
                    _items = objArray;
                }
                else
                {
                    _items = EmptyArray;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                if ((uint) index >= (uint) Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return _items[index];
            }
            set
            {
                if ((uint) index >= (uint) Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                _items[index] = value;
            }
        }


        public DynamicList()
        {
            _items = EmptyArray;
        }

        public DynamicList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }
            _items = capacity == 0 ? EmptyArray : new T[capacity];
        }


        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void Add(T item)
        {
            if (Count == _items.Length)
            {
                EnsureCapacity(Count + 1);
            }
            var items = _items;
            var size = Count;
            Count = size + 1;
            var index = size;
            var obj = item;
            items[index] = obj;
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            RemoveAt(index);

            return true;
        }

        public void RemoveAt(int index)
        {
            if ((uint) index >= (uint) Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            Count = Count - 1;
            if (index < Count)
            {
                Array.Copy(_items, index + 1, _items, index, Count - index);
            }
            _items[Count] = default(T);
        }

        public void Clear()
        {
            if (Count <= 0)
            {
                return;
            }
            Array.Clear(_items, 0, Count);
            Count = 0;
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            if (array != null && array.Rank != 1)
            {
                throw new ArgumentException(nameof(array.Rank));
            }
            try
            {
                Array.Copy(_items, 0, array, arrayIndex, Count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException(nameof(array));
            }
        }


        private void EnsureCapacity(int min)
        {
            if (_items.Length >= min)
            {
                return;
            }
            var num = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;
            if ((uint) num > 2146435071U)
            {
                num = 2146435071;
            }
            if (num < min)
            {
                num = min;
            }
            Capacity = num;
        }

        private int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, Count);
        }



        public struct Enumerator : IEnumerator<T>
        {
            private readonly DynamicList<T> _list;

            private int _index;


            public T Current { get; private set; }


            object IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || _index == _list.Count + 1)
                    {
                        throw new InvalidOperationException(nameof(Current));
                    }

                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                _index = 0;
                Current = default(T);
            }


            internal Enumerator(DynamicList<T> list)
            {
                _list = list;
                _index = 0;
                Current = default(T);
            }


            public bool MoveNext()
            {
                var list = _list;
                if ((uint) _index >= (uint) list.Count)
                {
                    return MoveNextRare();
                }
                Current = list._items[_index];
                _index = _index + 1;

                return true;
            }

            public void Dispose()
            {

            }


            private bool MoveNextRare()
            {
                _index = _list.Count + 1;
                Current = default(T);
                {
                    return false;
                }
            }
        }
    }
}