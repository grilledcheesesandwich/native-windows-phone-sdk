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
using Esmann.WP.Common.Virtualizing;
using System.Threading;
using EtaSDK.Utils;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using esmann.WP.Common.ViewModels;
using Esmann.WP.Common.Networking;

namespace EtaSampleApp.ViewModels
{
    public class CatalogPage : ViewModelBase, IVirtualItem//, IDisposable
    {
        public int Id { get; set; }

        private string _pageUri = "/Images/bg.png";
        public string PageUri
        {
            get { return _pageUri; }
            set
            {
                if (value != _pageUri)
                {
                    _pageUri = value.Replace("/", "\\");
                    NotifyPropertyChanged(()=>PageUri);
                }
            }
        }

        private string _zoomPageUri;
        public string ZoomPageUri
        {
            get { return _zoomPageUri; }
            set
            {
                if (value != _zoomPageUri)
                {
                    _zoomPageUri = value;
                    NotifyPropertyChanged(() => ZoomPageUri);
                }
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    NotifyPropertyChanged(()=>IsSelected);
                }
            }
        }

        //~CatalogPage()
        //{
        //    Dispose();
        //}
        //bool isDisposeing = false;

        //public void Dispose()
        //{
        //    if (!isDisposeing)
        //    {
        //        isDisposeing = true;
        //        int count = base.getEventsCount();
        //        base.RemoveAllEvents();
        //        PageUri = null;
        //        Debug.WriteLine(String.Format("Disposing item# {0} event count: {1}", Id, count));
        //    }
        //}
    }

    public class CatalogPages : VirtualizingListBase<CatalogPage>
    {
        public string CatalogId { get; private set; }
        private IsoStorageHelper isoStore = new IsoStorageHelper();
        private IsolatedStorageCacheHelper isoCache = new IsolatedStorageCacheHelper();
        public CatalogPages(string catalogId,int count, int cacheSize)
        {
            
            CatalogId = catalogId;
            Count = count;
            CacheSize = cacheSize;
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
                isoStore.DeleteDiretory("Catalogs");
                isoStore.CreateDirectory("Catalogs/" + catalogId + "/view");
            //}
        }
        private object syncContext = new object();
        public override void OnLoadItem(CatalogPage item, int index)
        {
            var path = string.Format("Catalogs/{0}/view/{1}.jpg", CatalogId, index + 1);
            string uri = CatalogPageHelper.GetPageUri(CatalogId, (index + 1).ToString(),ImageResolution.View);

            isoCache.FetchResource(path, new Uri(uri), isoPath => item.PageUri = isoPath);
            base.OnLoadItem(item, index);
        }
    }
}
