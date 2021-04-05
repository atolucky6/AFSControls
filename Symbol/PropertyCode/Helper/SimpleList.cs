using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyCode
{
    public class SimpleList : IList
    {
        public List<object> _contents = new List<object>();
        private int _count;

        public SimpleList()
        {
            _count = 0;
        }

        // IList Members
        public int Add(object value)
        {
            _contents.Add(value);
            return _contents.Count;
        }

        public void Clear()
        {
            _count = 0;
            _contents.Clear();
        }

        public bool Contains(object value)
        {
            bool inList = false;
            for (int i = 0; i < Count; i++)
            {
                if (_contents[i] == value)
                {
                    inList = true;
                    break;
                }
            }
            return inList;
        }

        public int IndexOf(object value)
        {
            int itemIndex = -1;
            for (int i = 0; i < Count; i++)
            {
                if (_contents[i] == value)
                {
                    itemIndex = i;
                    break;
                }
            }
            return itemIndex;
        }

        public void Insert(int index, object value)
        {
            if ((_count + 1 <= _contents.Count) && (index < Count) && (index >= 0))
            {
                _count++;

                for (int i = Count - 1; i > index; i--)
                {
                    _contents[i] = _contents[i - 1];
                }
                _contents[index] = value;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Remove(object value)
        {
            _contents.Remove(value);
        }

        public void RemoveAt(int index)
        {
            if ((index >= 0) && (index < Count))
            {
                for (int i = index; i < Count - 1; i++)
                {
                    _contents[i] = _contents[i + 1];
                }
                _count--;
            }
        }

        public object this[int index]
        {
            get
            {
                return _contents[index];
            }
            set
            {
                _contents[index] = value;
            }
        }

        // ICollection Members

        public void CopyTo(Array array, int index)
        {
            int j = index;
            for (int i = 0; i < Count; i++)
            {
                array.SetValue(_contents[i], j);
                j++;
            }
        }

        public int Count
        {
            get
            {
                return _contents.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        // Return the current instance since the underlying store is not
        // publicly available.
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public void PrintContents()
        {
            Console.WriteLine("List has a capacity of {0} and currently has {1} elements.", _contents.Count, _count);
            Console.Write("List contents:");
            for (int i = 0; i < Count; i++)
            {
                Console.Write(" {0}", _contents[i]);
            }
            Console.WriteLine();
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
