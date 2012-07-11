﻿using System;
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
using System.Linq;
using EtaSampleApp.ViewModels;
using System.Threading.Tasks;

namespace EtaSampleApp
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            this.Catalogs = new ObservableCollection<Catalog>();
            this.Stores = new ObservableCollection<Store>();
            this.OffersSearch = new ObservableCollection<Offer>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<Catalog> Catalogs { get; private set; }
        public ObservableCollection<Store> Stores { get; private set; }

        public ObservableCollection<Offer> OffersSearch { get; private set; }
        
        public bool IsDataLoaded
        {
            get;
            private set;
        }

        private bool isUserDataLoaded = false;
        public bool IsUserDataLoaded
        {
            get
            {
                return isUserDataLoaded;
            }
            set
            {
                if (value != isUserDataLoaded)
                {
                    isUserDataLoaded = value;
                    if (IsUserDataLoaded)
                    {
                        LoadEtaCatalogList();
                        LoadEtaStoreList();
                    }
                    this.NotifyPropertyChanged(() => IsUserDataLoaded);
                }
            }
        }

        private bool isSearching = false;
        public bool IsSearching
        {
            get
            {
                return isSearching;
            }
            set
            {
                if (value != isSearching)
                {
                    isSearching = value;

                    this.NotifyPropertyChanged(() => IsSearching);
                }
            }
        }

        private UserViewModel userViewModel = null;
        public UserViewModel UserViewModel
        {
            get
            {
                return userViewModel;
            }
            set
            {
                if (value != userViewModel)
                {
                    userViewModel = value;
                    this.NotifyPropertyChanged(() => UserViewModel);
                }
            }
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
            if (string.IsNullOrWhiteSpace(q) || IsSearching)
            {
                return;
            }
            IsSearching = true;
            var userModel = UserViewModel;// App.ViewModel.UserViewModel;
            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm(EtaApiConstants.EtaApi_Latitude, userModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, userModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, userModel.Location.IsGeoCoded ? "0" : "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");//userModel.Location.Accuracy.ToString());
            options.AddParm(EtaApiConstants.EtaApi_Ditance, userModel.Distance.ToString());
            
            OffersSearch.Clear();            
            var api = new EtaSDKv2();
            api.GetOfferSearch(options, 
                q, 
                result => {
                    if (result != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => { 
                            var byDistance = result.OrderBy(item => int.Parse(item.Store.Distance));                        Deployment.Current.Dispatcher.BeginInvoke(() => { 
                                foreach (var item in byDistance)
                                {
                                    var dis = item.Store.Distance;
                                    OffersSearch.Add(item);
                                }
                            });
                        });
                    }
                    IsSearching = false;
                },
                (error,uri) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var msg = "Ups! Kunne ikke gennemfører søgning";
#if DEBUG
                        msg += " " + error.Message + " " + uri.ToString(); 

#endif
                        MessageBox.Show(msg);
                        IsSearching = false;

                    });
                });
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            InitializeModel();
        }

        private async void InitializeModel()
        {
            UserViewModel = await UserViewModel.LoadModelAsync();

            //UserViewModel
            if (UserViewModel != null && UserViewModel.Location != null && UserViewModel.Location.IsValid)
            {
                IsUserDataLoaded = true;
            }
        }

        public void LoadEtaCatalogList()
        {
            var userModel = UserViewModel;
            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm(EtaApiConstants.EtaApi_Latitude, userModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, userModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, userModel.Location.IsGeoCoded ? "0" : "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");//userModel.Location.Accuracy.ToString());
            options.AddParm(EtaApiConstants.EtaApi_Ditance, userModel.Distance.ToString());

            var api = new EtaSDKv2();
            api.GetCatalogList(options, catalogs =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { 
                    foreach (var catalog in catalogs)
                    {
                        this.Catalogs.Add(catalog);
                    }
                    IsDataLoaded = true;
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

        public void LoadEtaStoreList()
        {
            var userModel = UserViewModel;
            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm(EtaApiConstants.EtaApi_Latitude, userModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, userModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, userModel.Location.IsGeoCoded ? "0" : "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");//userModel.Location.Accuracy.ToString());
            options.AddParm(EtaApiConstants.EtaApi_Ditance, userModel.Distance.ToString());

            var api = new EtaSDKv2();
            api.GetStoreList(options, stores =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    foreach (var store in stores)
                    {
                        this.Stores.Add(store);
                    }
                    //IsDataLoaded = true;
                });

            }, (error, uri) =>
            {
                var msg = error.Message;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
#if DEBUG
                    MessageBox.Show(msg + "\n" + uri, "Exception", MessageBoxButton.OK);
#endif
                    //IsDataLoaded = false;
                });
            });
        }
    }
}