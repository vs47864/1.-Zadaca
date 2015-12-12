using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonoGame
{
    public class IntegerList : IIntegerList
    {
        private int[] _internalStorage;
        public int i;
        public int Countt = 0;

        public IntegerList()
        {
            _internalStorage = new int[4];
        }
        public IntegerList(int initialSize)
        {
            _internalStorage = new int[initialSize];
        }

        public void Add(int item)
        {
            int y = _internalStorage.Length;
            if (i >= y)
            {
                int[] _temp = new int[y * 2];
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

        public bool Remove(int item)
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

        public int GetElement(int index)
        {
            if (index < _internalStorage.Length)
            {
                int item = _internalStorage[index];
                return item;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public int IndexOf(int item)
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

        public bool Contains(int item)
        {
            int index = Array.IndexOf(_internalStorage, item);
            if (index == -1)
            {
                return false;
            }
            else return true;

        }

    }
}
