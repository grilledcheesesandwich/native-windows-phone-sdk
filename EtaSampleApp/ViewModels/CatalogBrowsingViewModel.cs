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
using esmann.WP.Common.ViewModels;
using System.Linq;
using EtaSDK.ApiModels;
using Esmann.WP.Common.Virtualizing;
using EtaSDK.Utils;
using Esmann.WP.Common.Networking;

namespace EtaSampleApp.ViewModels
{
    public class CatalogBrowsingViewModel : ViewModelBase
    {
        public Catalog Catalog { get; private set; }

        private CatalogPagesList _pages;
        public CatalogPagesList Pages
        {
            get { return _pages; }
            private set 
            {
                if (_pages != value)
                {
                    _pages = value;
                    NotifyPropertyChanged(() => Pages);
                }
            }
        }
        
        public CatalogBrowsingViewModel(string catalogId)
        {
            Catalog = App.ViewModel.Catalogs.Where(item => item.Id == catalogId).FirstOrDefault();
            if (Catalog == null)
            {
                throw new ArgumentOutOfRangeException("Unkonw Catalog");
            }
        }
        public bool IsLoaded { get; private set; }
        public void LoadData()
        {
            if (IsLoaded)
            {
                return;
            }

            Pages = new CatalogPagesList(Catalog, 0);
            IsLoaded = true;
        }

        CatalogPageItem _selectedPageItem;
        public CatalogPageItem SelectedPageItem
        {
            get { return _selectedPageItem; }
            set
            {
                if (value != _selectedPageItem)
                {
                    _selectedPageItem = value;
                    LoadZoomImage(value);
                    NotifyPropertyChanged(() => SelectedPageItem);
                }
            }
        }
        private IsolatedStorageCacheHelper isoCache = new IsolatedStorageCacheHelper();
        private void LoadZoomImage(CatalogPageItem value)
        {
            if (value == null)
            {
                return;
            }
            int pageIndex = value.Id + 1;
            var path = string.Format("Catalogs/{0}/zoom/{1}.jpg", Catalog.Id, pageIndex);
            string uri = String.Format(Catalog.Images.Zoom.Replace("%d", "{0}"), pageIndex);
            isoCache.FetchResource(path, new Uri(uri), isoPath => value.ZoomUri = isoPath);
        }
    }

    public class CatalogPageItem : ViewModelBase, IVirtualItem
    {
        public int Id { get; set; }

        private string _thumbUri = "/Images/bg.png";
        public string ThumbUri
        {
            get { return _thumbUri; }
            set
            {
                if (value != _thumbUri)
                {
                    _thumbUri = value;
                    NotifyPropertyChanged(() => ThumbUri);
                }
            }
        }

        private string _viewUri = "/Images/bg.png";
        public string ViewUri
        {
            get { return _viewUri; }
            set
            {
                if (value != _viewUri)
                {
                    _viewUri = value;
                    NotifyPropertyChanged(() => ViewUri);
                }
            }
        }

        private string _zoomUri = "/Images/bg.png";
        public string ZoomUri
        {
            get { return _zoomUri; }
            set
            {
                if (value != _zoomUri)
                {
                    _zoomUri = value;
                    NotifyPropertyChanged(() => ZoomUri);
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
                    NotifyPropertyChanged(() => IsSelected);
                }
            }
        }
       
    }

    public class CatalogPagesList : VirtualizingListBase<CatalogPageItem>
    {
        public Catalog Catalog { get; private set; }
        private IsoStorageHelper isoStore = new IsoStorageHelper();
        private IsolatedStorageCacheHelper isoCache = new IsolatedStorageCacheHelper();
        public CatalogPagesList(Catalog catalog, int cacheSize)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException("Catalog");
            }
            Catalog = catalog;
            int count;
            if (int.TryParse(Catalog.PageCount,out count))
            {
                Count = count;
            }
            CacheSize = cacheSize;
        }
        private object syncContext = new object();

        public override void OnLoadItem(CatalogPageItem item, int index)
        {
            int pageIndex = index + 1;
            var path = string.Format("Catalogs/{0}/view/{1}.jpg", Catalog.Id, pageIndex);
            string uri = String.Format(Catalog.Images.View.Replace("%d", "{0}"), pageIndex);
            //string uri = CatalogPageHelper.GetPageUri(CatalogId, (index + 1).ToString(), ImageResolution.View);

            isoCache.FetchResource(path, new Uri(uri), isoPath => item.ViewUri = isoPath);
            base.OnLoadItem(item, index);
        }
    }
}
