using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonoGame
{
    class GenericList<X> : IGenericList<X>
    {

        private X[] _internalStorage = new X[1];

        public int i;
        public int Countt = 0;

        public void Add(X item)
        {
            int y = _internalStorage.Length;
            if (i >= y)
            {
                X[] _temp = new X[y * 2];
                for (int i = 0; i < y; i++)
                {
                    _temp[i] = _internalStorage[i];
                }
                _internalStorage = _temp;
            }
            _internalStorage[i] = item;
            i++;
            Countt++;
        }

        public bool Remove(X item)
        {
            int index = Array.IndexOf(_internalStorage, item);
            if (index != -1)
            {
                _internalStorage = _internalStorage.Where((val, idx) => idx != index).ToArray();
                Countt--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveAt(int index)
        {
            if (index > Countt)
                return false;
            else return Remove(_internalStorage[index]);
        }

        public X GetElement(int index)
        {
            if (index < _internalStorage.Length)
            {
                X item = _internalStorage[index];
                return item;
            }
            else
            {
                throw new IndexOutOfRangeException("Premašili smo");
            }
        }

        public int IndexOf(X item)
        {
            int index = Array.IndexOf(_internalStorage, item);
            return index;
        }

        public int Count
        {
            get { return Countt; }

        }

        public void Clear()
        {
            Countt = 0;
        }

        public bool Contains(X item)
        {
            int index = Array.IndexOf(_internalStorage, item);
            if (index == -1)
            {
                return false;
            }
            else return true;

        }

        public IEnumerator<X> GetEnumerator()
        {
            return new GenericListEnumerator<X>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
