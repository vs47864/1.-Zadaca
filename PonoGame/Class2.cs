using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonoGame
{
    class GenericListEnumerator<T> : IEnumerator<T>
    {

        public IGenericList<T> _collection;
        int state = -1;
        public GenericListEnumerator(IGenericList<T> collection)
        {
            _collection = collection;
        }

        public bool MoveNext()
        {
            state++;
            if (state > _collection.Count) return true;
            else return false;
        }

        public T Current
        {
            get
            {
                return _collection.GetElement(state);

            }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
        }

        public void Reset()
        {
        }
    }
}
