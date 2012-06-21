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
            this.OffersSearch = new ObservableCollection<Offer>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<Catalog> Catalogs { get; private set; }
        public ObservableCollection<Offer> OffersSearch { get; private set; }
        
        public bool IsDataLoaded
        {
            get;
            private set;
        }

        private string offerSearchQueryText = "kaffe";
        public string OfferSearchQueryText
        {
            get
            {
                return offerSearchQueryText;
            }
            set
            {
                if (value != offerSearchQueryText)
                {
                    offerSearchQueryText = value;
                    this.NotifyPropertyChanged(() => OfferSearchQueryText);
                }
            }
        }

        private Offer selectedOffer;
        public Offer SelectedOffer
        {
            get
            {
                return selectedOffer;
            }
            set
            {
                if (value != selectedOffer)
                {
                    selectedOffer = value;
                    this.NotifyPropertyChanged(() => SelectedOffer);
                }
            }
        }

        public void LoadOfferSearchResult(string q){
            if (string.IsNullOrWhiteSpace(q))
            {
                return;
            }
            OffersSearch.Clear();            
            var api = new EtaSDKv2();
            api.GetOfferSearch(null, 
                q, 
                result => {
                    if (result != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => { 
                            foreach (var item in result)
                            {
                                OffersSearch.Add(item);
                            }
                        });
                    }
                },
                error =>
                {
                    MessageBox.Show("Ups! Kunne ikke gennemfører søgning");
                });
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

            }, (error,uri) => {
                var msg = error.Message;
                Deployment.Current.Dispatcher.BeginInvoke(() => {
#if DEBUG
                    MessageBox.Show(msg + "\n"+uri, "Exception", MessageBoxButton.OK);           
#endif
                    IsDataLoaded = false;
                });
            });
        }
    }
}