using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Diagnostics;
using System.Windows.Threading;

namespace Esmann.WP.Common.Virtualizing
{
    public class VirtualizingListBase<T> : /* INotifyPropertyChanged, INotifyCollectionChanged,*/ IList<T>, IList where T : class, IVirtualItem, new()
    {
        private int _count;
        /// <summary>
        /// Return the total number of items in your list.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (value != _count)
                {
                    _count = value;
                    NotifyPropertyChanged("Count");
                    NotifyCollectionChanged();
                }
            }
        }
        
        public int CacheSize { get; protected set; }

        public VirtualizingListBase() : this(1, 2)
        {

        }

        public VirtualizingListBase(int count, int cacheSize)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("Count should be > 0");
            }
            Count = count;

            if (cacheSize < 0)
            {
                throw new ArgumentOutOfRangeException("cache size should >= 0");
            }
            CacheSize = cacheSize;
        }

        #region LocalCache
        private List<T> internalCache = new List<T>();

        private T CacheFetch(int index)
        {
            if (CacheSize == 0)
            {
                return default(T);
            }
            return internalCache.Where(item => item.Id == index).FirstOrDefault();
        }

        private void CacheAdd(T item)
        {
            if (CacheSize == 0)
            {
                return;
            }

            var cacheItem = internalCache.Where(citem => citem.Id == item.Id).FirstOrDefault();
            if (cacheItem != null)
            {
                return;
            }
            internalCache.Add(item);
            if (internalCache.Count > CacheSize)
            {
                internalCache.RemoveAt(0);
            }
        }

        #endregion

        #region IList<T> Members

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get
            {
                return (this as IList)[index] as T;
                //throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            internalCache.Clear();
            //throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

       

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            //throw new NotImplementedException();

            for (int i = 0; i < Count; i++)
            {
                yield return (this as IList)[i] as T;
            }
            //sadf
        }

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public virtual int ConvertObjectToIndexOf(object value)
        {
            return 0;
        }
        

        public int IndexOf(object value)
        {
            return ConvertObjectToIndexOf(value);
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        object IList.this[int index]
        {
            get
            {
                if (index > Count)
                {
                    return null;
                }
                // here is where the magic happens...
                // create/load your data on the fly 
                // or 
                // from the cache.
                if (CacheSize > 0)
                {
                    var cacheItem = CacheFetch(index);
                    if (cacheItem != null)
                    {
                        Debug.WriteLine("Cache Lookup - item found: " + index.ToString());
                        return cacheItem;
                    }
                    Debug.WriteLine("Cache Lookup - item not found: " + index.ToString());

                }
                Debug.WriteLine("Requsted item " + index.ToString());
                var item = new T() { Id = index };
                OnLoadItem(item, index);
                Debug.WriteLine(string.Format("OnLoadItem called for item# {0}", index));
                return item;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual void OnLoadItem(T item, int index){
            Done(item);
        }

        private void Done(T item)
        {
            // NOOP || Cache Item
            if (CacheSize > 0)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    CacheAdd(item);
                });
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (this is INotifyPropertyChanged)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    PropertyChangedEventHandler handler = PropertyChanged;
                    if (null != handler)
                    {
                        handler(this, new PropertyChangedEventArgs(propertyName));
                    }
                });
            }
        }
        #endregion

        #region NotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void NotifyCollectionChanged()
        {
            if (this is INotifyCollectionChanged)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NotifyCollectionChangedEventHandler handler = CollectionChanged;
                    if (null != handler)
                    {
                        handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }
                });
            }
        }
        #endregion
    }
}
