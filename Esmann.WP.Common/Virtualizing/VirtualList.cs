using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using System.Threading;
using System.Windows;
using System.Linq;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Info;

namespace Esmann.WP.Common.Virtualizing
{
    public class VirtualList<T> : INotifyPropertyChanged, INotifyCollectionChanged, IList<T>, IList where T : class, IVirtualItem, new()
    {
        public VirtualList() : this(1, 2)
        {

        }
        private int _count = 0;
        public VirtualList(int count, int cacheSize)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("Count should be > 0");
            }
            _count = count;

            if (cacheSize < 0)
            {
                throw new ArgumentOutOfRangeException("cache size should >= 0");
            }
            _cacheSize = cacheSize;
        }

        #region LocalCache
        private List<T> _cache = new List<T>();
        private int _cacheSize = 0;

        private T CacheFetch(int index){
            if (_cacheSize == 0)
            {
                return default(T);
            }
            return _cache.Where(item => item.Id == index).FirstOrDefault();
        }

        private void CacheAdd(T item)
        {
            if (_cacheSize == 0)
            {
                return;
            }
            
            var cacheItem = _cache.Where(citem => citem.Id == item.Id).FirstOrDefault();
            if (cacheItem != null)
            {
                return;
            }
            _cache.Add(item);
            if (_cache.Count > _cacheSize)
            {
                _cache.RemoveAt(0);
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
                throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

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
                    Deployment.Current.Dispatcher.BeginInvoke(() => {
                        NotifyPropertyChanged("Count");
                        NotifyCollectionChanged();
                    });
                }
            }
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
            throw new NotImplementedException();

            //for (int i = 0; i < Count; ++i)
            //{
            //    yield return new T();
            //}
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

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
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
                var cacheItem = CacheFetch(index);
                if (cacheItem != null)
                {
                    return cacheItem;
                }
                Debug.WriteLine("Requsted item " + index.ToString());
                var item = new T() { Id = index, Name="Loading..." };
                LoadItem(Done, index, item);
                return item;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private void Done(T item)
        {
            // NOOP || Cache Item
            Deployment.Current.Dispatcher.BeginInvoke(() => {
                CacheAdd(item);
            });
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

        #region Threading
        private void LoadItem(Action<T> done, object param, T item)
        {
            var start = DateTime.Now.Ticks;
            ThreadPool.QueueUserWorkItem(state => {
                if (!IsOnlineDataLoaded)
                {
                    Debug.WriteLine("---------------------------------------");
                    Debug.WriteLine("FIRST RUN");
                    Debug.WriteLine("---------------------------------------");
                    LoadDataOnline();
                }
                
                // blocking call! ensure data is loaded and ready
                while (!IsOnlineDataLoaded)
                {
                    Thread.Sleep(15);
                }

                int index = (int)param;
                var theItem = _onlineData[index];
                var id = theItem.Id;
                var name = theItem.Name;
                //string name = string.Format("Long description {0} - (load time: {1}ms) ",index.ToString(),loadtime);
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    var end = DateTime.Now.Ticks;
                    var loadtime = TimeSpan.FromTicks(end - start).TotalMilliseconds;
                    //string name = string.Format("Long description {0} - (load time: {1}ms) ", index.ToString(), loadtime);                    
                    item.Name = name;
             
                });
                var mem = string.Format("mem: {0}  - Peak mem: {1}", Math.Round(DeviceStatus.ApplicationCurrentMemoryUsage / 1048576d,0), Math.Round(DeviceStatus.ApplicationPeakMemoryUsage / 1048576d,0));
                Debug.WriteLine(mem);
                done(item);
            });
        }
        
        #endregion

        #region Load Data Online

        private bool IsOnlineDataLoaded = false;
        private List<T> _onlineData = new List<T>();

        private void LoadDataOnline()
        {
            Debug.WriteLine("LoadDataOnline called");

            //LoadDataOnlineBingImageSearch();
            LoadDataOnlineSimulation();
        }

        private void LoadDataOnlineSimulation()
        {
            Debug.WriteLine("LoadDataOnline called");
            // emulate very long load and processing time
            Thread.Sleep(TimeSpan.FromSeconds(5));
            _onlineData.Clear();
            for (int i = 0; i < 10000; i++)
            {
                var item = new T() { Id = i, Name = "http://www.concept-phones.com/wp-content/uploads/2011/05/Nokia_Windows_Phone_7_Anna_1.jpg?tickker=" + i };
                _onlineData.Add(item);
            }
            Count = _onlineData.Count;
            IsOnlineDataLoaded = true;
            Debug.WriteLine("LoadDataOnline Done");
        }

        // http://www.bing.com/developers/s/APIBasics.html
        //http://www.bing.com/developers/s/APIBasics.html#_Using_the_API_1
        private void LoadDataOnlineBingImageSearch()
        {
            // image optimization:
            //http://blogs.msdn.com/b/swick/archive/2011/04/07/image-tips-for-windows-phone-7.aspx
            
            // listbox optimization:
            //http://blogs.msdn.com/b/ptorr/archive/2010/10/12/procrastination-ftw-lazylistbox-should-improve-your-scrolling-performance-and-responsiveness.aspx

            // json query:
            // http://api.bing.net/json.aspx?Appid=0AC145FC2BACF33CA1FD970101DA630D2D598C3A&query=windows+phone&sources=image&image.count=10&image.offset=100

            // rss query:
            // http://api.bing.com/rss.aspx?source=image&query=windows+phone&image.count=50&image.offset=50
            // http://api.bing.com/rss.aspx?source=image&query=windows+phone&image.count=50&image.offset=1000
            Debug.WriteLine("LoadDataOnlineBingImageSearch called");

            WebClient client = new WebClient();
            // bing Images Search from Windows Phone 
            client.OpenReadAsync(new Uri("http://www.bing.com/images/search?q=Windows+Phone&filt=all&view=large&FORM=VBCIRL#x0y50283"));
            client.OpenReadCompleted += client_OpenReadCompleted;
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            HtmlDocument doc = new HtmlDocument();
            using (var stream = e.Result)            
            {
                doc.Load(stream);
            }
            var imageUris = doc.DocumentNode.Descendants("img")
                .Where(node => node.Attributes["src"] != null )
                .Select(node => node.Attributes["src"].Value)
                .ToList();
            _onlineData.Clear();
            for (int i = 0; i < imageUris.Count; i++)
            {
                _onlineData.Add(new T() { Id = i, Name = imageUris[i] });
            }
            IsOnlineDataLoaded = true;

            Count = _onlineData.Count;
            Debug.WriteLine("LoadDataOnline Done");
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region NotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void NotifyCollectionChanged()
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (null != handler)
            {
                handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        #endregion

    }
}