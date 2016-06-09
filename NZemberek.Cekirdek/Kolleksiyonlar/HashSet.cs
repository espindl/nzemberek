using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace NZemberek.Cekirdek.Kolleksiyonlar
{
    /// <summary>
    /// HashSet implementasyonu
    /// </summary>
    public class HashSet<Type> : IEnumerable<Type>, ICollection
    {
        public static readonly HashSet<Type> EMPTY_SET = new HashSet<Type>();

        
        private Hashtable table;

        public HashSet():this(7){}

        public HashSet(int capacity)
        {
            table = new Hashtable(capacity);
        }

        public HashSet(Type[] array):this(array.Length)
        {
            this.AddAll(array);            
        }


        public void AddAll(ICollection items)
        {
            foreach (Type t in items)
            {
                this.Add(t);
            }
        }

        public bool Add(Type t)
        {
            if (!table.ContainsKey(t))
            {
                table.Add(t, null);
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Remove(Type t)
        {
            if (table.ContainsKey(t))
                table.Remove(t);
        }

        public bool Contains(Type t)
        {
            return table.ContainsKey(t);
        }

        public int Count
        {
            get { return table.Count; }
        }


        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return table.Keys.GetEnumerator();
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            table.Keys.CopyTo(array, index);
        }

        public bool IsSynchronized
        {
            get { return table.Keys.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return table.Keys.SyncRoot; }
        }

        #endregion

        #region IEnumerable<Type> Members

        public IEnumerator<Type> GetEnumerator()
        {
            return new TypedEnumerator(table.Keys.GetEnumerator());
        }

        #endregion

        private class TypedEnumerator : IEnumerator<Type>
        {
            private IEnumerator enumerator;

            public TypedEnumerator(IEnumerator aEnumerator)
            {
                enumerator = aEnumerator;
            }

            #region IEnumerator<Type> Members

            public Type Current
            {
                get { return (Type)enumerator.Current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return enumerator.Current; }
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }

            #endregion
        }
    }
}
