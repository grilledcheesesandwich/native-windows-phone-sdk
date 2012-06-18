using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using eta.sdk;
using EtaSDK;
using EtaSDK.Utils;
using EtaSDK.ApiModels;
using System.Linq.Expressions;
using System.Windows.Threading;
using esmann.WP.Common.ViewModels;


namespace EtaSampleApp
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            this.Catalogs = new ObservableCollection<Catalog>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<Catalog> Catalogs { get; private set; }
        
        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            LoadEtaCatalogList();
            IsDataLoaded = true;
        }

        private void LoadEtaCatalogList()
        {
            var api = new EtaSDKv2();
            api.GetCatalogList(null, catalogs =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { 
                    foreach (var catalog in catalogs)
                    {
                        this.Catalogs.Add(catalog);
                    }
                });

            }, error => {
                var msg = error.Message;
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    IsDataLoaded = false;
                });
            });
        }
    }
}