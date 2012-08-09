using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using esmann.WP.Common.ViewModels;
using EtaSampleApp.ViewModels;
using EtaSDK;
using EtaSDK.ApiModels;
using EtaSDK.Utils;
using EtaSDK.v3;
using System.Threading.Tasks;
using System.Device.Location;
using System.Threading;
using Esmann.WP.Common.Location;

namespace EtaSampleApp
{
    public class MainViewModel : ViewModelBase
    {
        EtaApi Api = null;
        public MainViewModel()
        {
            Api = new EtaApi();
            this.Catalogs = new ObservableCollection<Catalog>();
            this.Stores = new ObservableCollection<Store>();
            this.OffersSearch = new ObservableCollection<Offer>();
            this.SuggestedOffers = new ObservableCollection<Offer>();
        }

        #region Global ViewModel things

        public void LoadData()
        {
            //TaskEx.Run(()=>
            UpdateViewModel();
            //);
        }

        public bool IsDataLoaded
        {
            get { return IsSuggestedOffersLoaded && IsStoresLoaded && IsCatalogsLoaded; }
        }

        async internal void UpdateViewModel()
        {
            await LoadStores();
            await LoadCatalogs();
            await LoadSuggestedOffers();
            await LoadSearchOffers(OfferSearchQueryText);
            MaintainGpsLocation();
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

        #endregion

        #region Offers (suggested)
        public ObservableCollection<Offer> SuggestedOffers { get; private set; }

        private bool isSuggestedOffersLoaded = false;
        public bool IsSuggestedOffersLoaded
        {
            get
            {
                return isSuggestedOffersLoaded;
            }
            set
            {
                if (value != isSuggestedOffersLoaded)
                {
                    isSuggestedOffersLoaded = value;
                    this.NotifyPropertyChanged(() => IsSuggestedOffersLoaded);
                    this.NotifyPropertyChanged(() => IsDataLoaded);

                }
            }
        }

        private bool isSuggestedOffersLoading = false;
        public bool IsSuggestedOffersLoading
        {
            get
            {
                return isSuggestedOffersLoading;
            }
            set
            {
                if (value != isSuggestedOffersLoading)
                {
                    isSuggestedOffersLoading = value;
                    this.NotifyPropertyChanged(() => IsSuggestedOffersLoading);
                }
            }
        }

        async public Task LoadSuggestedOffers()
        {
            if (IsSuggestedOffersLoading)
            {
                return;
            }
            IsSuggestedOffersLoading = true;

            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm(EtaApiConstants.EtaApi_Latitude, UserViewModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, UserViewModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, UserViewModel.Location.IsGeoCoded ? "0" : "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");//userModel.Location.Accuracy.ToString());
            options.AddParm(EtaApiConstants.EtaApi_Ditance, UserViewModel.Distance.ToString());

            options.AddParm("type", "suggested");
            options.AddParm("from", UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm("to", UNIXTime.GetTimestamp(DateTime.Now.AddDays(14)));

            var response = await Api.GetOfferListAsync(options);
            if (SuggestedOffers.Any())
            {
                SuggestedOffers.Clear();
            }
            if (response.HasErrors)
            {
                IsSuggestedOffersLoaded = false;
            }
            else
            {
                foreach (var offer in response.Result)
                {
                    SuggestedOffers.Add(offer);
                }
                IsSuggestedOffersLoaded = true;
            }
            IsSuggestedOffersLoading = false;
        }

        #endregion

        #region Catalog List

        public ObservableCollection<Catalog> Catalogs { get; private set; }

        private bool isCatalogsLoaded = false;
        public bool IsCatalogsLoaded
        {
            get
            {
                return isCatalogsLoaded;
            }
            set
            {
                if (value != isCatalogsLoaded)
                {
                    isCatalogsLoaded = value;
                    this.NotifyPropertyChanged(() => IsCatalogsLoaded);
                    this.NotifyPropertyChanged(() => IsDataLoaded);
                }
            }
        }

        private bool isCatalogsLoading = false;
        public bool IsCatalogsLoading
        {
            get
            {
                return isCatalogsLoading;
            }
            set
            {
                if (value != isCatalogsLoading)
                {
                    isCatalogsLoading = value;
                    this.NotifyPropertyChanged(() => IsCatalogsLoading);
                }
            }
        }

        async public Task LoadCatalogs()
        {
            if (IsCatalogsLoading || !IsUserViewModelLoaded)
            {
                return;
            }
            IsCatalogsLoading = true;

            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm(EtaApiConstants.EtaApi_Latitude, UserViewModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, UserViewModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, UserViewModel.Location.IsGeoCoded ? "0" : "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");//userModel.Location.Accuracy.ToString());
            options.AddParm(EtaApiConstants.EtaApi_Ditance, UserViewModel.Distance.ToString());

            var response = await Api.GetCatalogListAsync(options);
            if (Catalogs.Any())
            {
                Catalogs.Clear();
            } 
            
            if (response.HasErrors)
            {
                IsCatalogsLoaded = false;
            }
            else
            {
                foreach (var catalog in response.Result)
                {
                    Catalogs.Add(catalog);
                }
                IsCatalogsLoaded = true;
            }
            IsCatalogsLoading = false;
        }

        #endregion

        #region Stores List

        public ObservableCollection<Store> Stores { get; private set; }

        private bool isStoresLoaded = false;
        public bool IsStoresLoaded
        {
            get
            {
                return isStoresLoaded;
            }
            set
            {
                if (value != isStoresLoaded)
                {
                    isStoresLoaded = value;
                    this.NotifyPropertyChanged(() => IsStoresLoaded);
                    this.NotifyPropertyChanged(() => IsDataLoaded);
                }
            }
        }

        private bool isStoresLoading = false;
        public bool IsStoresLoading
        {
            get
            {
                return isStoresLoading;
            }
            set
            {
                if (value != isStoresLoading)
                {
                    isStoresLoading = value;
                    this.NotifyPropertyChanged(() => IsStoresLoading);
                }
            }
        }

        async public Task LoadStores()
        {
            if (IsStoresLoading)
            {
                return;
            }
            IsStoresLoading = true;

            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm(EtaApiConstants.EtaApi_Latitude, UserViewModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, UserViewModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, UserViewModel.Location.IsGeoCoded ? "1" : "1");
            options.AddParm(EtaApiConstants.EtaApi_Ditance, UserViewModel.Distance.ToString());

            var response = await Api.GetStoreListAsync(options);
            if (Stores.Any())
            {
                Stores.Clear();
            }
            if (response.HasErrors)
            {
                IsStoresLoaded = false;
            }
            else
            {
                foreach (var store in response.Result)
                {
                    Stores.Add(store);
                }
                IsStoresLoaded = true;
            }
            IsStoresLoading = false;
        }

        #endregion

        #region Offers (search)
        public ObservableCollection<Offer> OffersSearch { get; private set; }

        private bool isSearchLoaded = false;
        public bool IsSearchLoaded
        {
            get
            {
                return isSearchLoaded;
            }
            set
            {
                if (value != isSearchLoaded)
                {
                    isSearchLoaded = value;
                    this.NotifyPropertyChanged(() => IsSearchLoaded);
                }
            }
        }

        private bool isSearchOffersLoading = false;
        public bool IsSearchOffersLoading
        {
            get
            {
                return isSearchOffersLoading;
            }
            set
            {
                if (value != isSearchOffersLoading)
                {
                    isSearchOffersLoading = value;
                    this.NotifyPropertyChanged(() => IsSearchOffersLoading);
                }
            }
        }

        private string offerSearchQueryText = "";
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
                    LoadSearchOffers(OfferSearchQueryText);
                }
            }
        }

        async public Task LoadSearchOffers(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return;
            }

            if (IsSearchOffersLoading)
            {
                return;
            }
            IsSearchOffersLoading = true;

            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm(EtaApiConstants.EtaApi_Latitude, UserViewModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, UserViewModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, UserViewModel.Location.IsGeoCoded ? "0" : "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "1");//userModel.Location.Accuracy.ToString());
            options.AddParm(EtaApiConstants.EtaApi_Ditance, UserViewModel.Distance.ToString());

            var response = await Api.GetOfferSearchAsync(options, q);
            if (OffersSearch.Any())
            {
                OffersSearch.Clear();
            }
            if (response.HasErrors)
            {
                IsSearchLoaded = false;
            }
            else
            {
                foreach (var offer in response.Result)
                {
                    OffersSearch.Add(offer);
                }
                IsSearchLoaded = true;
            }
            IsSearchOffersLoading = false;
        }

        #endregion

        #region UserViewModel (loading and maintenance)
        // Init + GPS update + save + load etc.
        private bool isUserViewModelLoaded = false;
        public bool IsUserViewModelLoaded
        {
            get
            {
                return isUserViewModelLoaded;
            }
            set
            {
                if (value != isUserViewModelLoaded)
                {
                    isUserViewModelLoaded = value;
                    this.NotifyPropertyChanged(() => IsUserViewModelLoaded);
                }
            }
        }

        private bool isUserViewModelLoading = false;
        public bool IsUserViewModelLoading
        {
            get
            {
                return isUserViewModelLoading;
            }
            set
            {
                if (value != isUserViewModelLoading)
                {
                    isUserViewModelLoading = value;
                    this.NotifyPropertyChanged(() => IsUserViewModelLoading);
                }
            }
        }
        
        private UserViewModel userViewModel = null;
        public UserViewModel UserViewModel
        {
            get
            {
                if (userViewModel == null)
                {
                    LoadUserViewModel();
                }
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

        async public Task LoadUserViewModel()
        {
            if (IsUserViewModelLoading)
            {
                return;
            }
            IsUserViewModelLoading = true;

            UserViewModel = await UserViewModel.LoadModelAsync();

            IsUserViewModelLoaded = UserViewModel != null && UserViewModel.Location != null && UserViewModel.Location.IsValid;
            IsUserViewModelLoading = false;
        }
        
        private bool IsMaintainGpsLocationRunning = false;

        private void MaintainGpsLocation()
        {
            if (IsMaintainGpsLocationRunning)
            {
                return;
            }
            GPSHelper gps = new GPSHelper();
            var lastLocation = new GeoCoordinate(UserViewModel.Location.Latitude, UserViewModel.Location.Longitude);
            TaskEx.Run(async () =>
            {
                IsMaintainGpsLocationRunning = true;
                while (UserViewModel.AllowGPS)
                {
                    var location = await gps.GetPositionAsync();
                    if (location != null && !location.IsUnknown)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            UserViewModel.Location.Latitude = location.Latitude;
                            UserViewModel.Location.Longitude = location.Longitude;
                            UserViewModel.Location.IsGeoCoded = false;
                            UserViewModel.Location.Timestamp = DateTime.Now.Ticks;
                            if (location.GetDistanceTo(lastLocation) > 300)
                            {
                                lastLocation = new GeoCoordinate(UserViewModel.Location.Latitude, UserViewModel.Location.Longitude);
                                UpdateViewModel();
                            }
                        });
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(60));
                }
                IsMaintainGpsLocationRunning = false;
            });
        }
        #endregion


        #region old code
       
        //private bool isUserDataLoaded = false;
        //public bool IsUserDataLoaded
        //{
        //    get
        //    {
        //        return isUserDataLoaded;
        //    }
        //    set
        //    {
        //        if (value != isUserDataLoaded)
        //        {
        //            isUserDataLoaded = value;
        //            this.NotifyPropertyChanged(() => IsUserDataLoaded);
        //        }
        //    }
        //}

        //private bool isSearching = false;
        //public bool IsSearching
        //{
        //    get
        //    {
        //        return isSearching;
        //    }
        //    set
        //    {
        //        if (value != isSearching)
        //        {
        //            isSearching = value;

        //            this.NotifyPropertyChanged(() => IsSearching);
        //        }
        //    }
        //}

        //private bool isStoresUpdateing = true;
        //public bool IsStoresUpdateing
        //{
        //    get
        //    {
        //        return isStoresUpdateing;
        //    }
        //    set
        //    {
        //        if (value != isStoresUpdateing)
        //        {
        //            isStoresUpdateing = value;

        //            this.NotifyPropertyChanged(() => IsStoresUpdateing);
        //        }
        //    }
        //}

        //private bool isCatalogsUpdateing = true;
        //public bool IsCatalogsUpdateing
        //{
        //    get
        //    {
        //        return isCatalogsUpdateing;
        //    }
        //    set
        //    {
        //        if (value != isCatalogsUpdateing)
        //        {
        //            isCatalogsUpdateing = value;

        //            this.NotifyPropertyChanged(() => IsCatalogsUpdateing);
        //        }
        //    }
        //}

        
       

       

        //async public void LoadOfferSearchResult2(string q)
        //{
        //    if (string.IsNullOrWhiteSpace(q) || IsSearching)
        //    {
        //        return;
        //    }
        //    IsSearching = true;
        //    OffersSearch.Clear();

        //    var userModel = UserViewModel;// App.ViewModel.UserViewModel;
        //    var options = new EtaApiQueryStringParameterOptions();
        //    options.AddParm(EtaApiConstants.EtaApi_Latitude, userModel.Location.Latitude.ToString("0.00000"));
        //    options.AddParm(EtaApiConstants.EtaApi_Longitude, userModel.Location.Longitude.ToString("0.00000"));
        //    options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
        //    options.AddParm(EtaApiConstants.EtaApi_Geocoded, userModel.Location.IsGeoCoded ? "0" : "0");
        //    options.AddParm(EtaApiConstants.EtaApi_Accuracy, "1");//userModel.Location.Accuracy.ToString());
        //    options.AddParm(EtaApiConstants.EtaApi_Ditance, userModel.Distance.ToString());

        //    var result = await Api.GetOfferSearchAsync(options, q);
        //    if (!result.HasErrors)
        //    {
        //        var byDistance = result.Result.OrderBy(item => int.Parse(item.Store.Distance));
        //        foreach (var item in byDistance)
        //        {
        //            OffersSearch.Add(item);
        //        }
        //    }
        //    else
        //    {
        //        var msg = "Ups! Kunne ikke gennemfører søgning";
        //        msg += " " + result.Error.Message;

        //        MessageBox.Show(msg);
        //        IsSearching = false;
        //    }
        //    IsSearching = false;
        //}
//        public void LoadOfferSearchResult(string q){
//            if (string.IsNullOrWhiteSpace(q) || IsSearching)
//            {
//                return;
//            }
//            IsSearching = true;
//            OffersSearch.Clear();            

//            var userModel = UserViewModel;// App.ViewModel.UserViewModel;
//            var options = new EtaApiQueryStringParameterOptions();
//            options.AddParm(EtaApiConstants.EtaApi_Latitude, userModel.Location.Latitude.ToString("0.00000"));
//            options.AddParm(EtaApiConstants.EtaApi_Longitude, userModel.Location.Longitude.ToString("0.00000"));
//            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
//            options.AddParm(EtaApiConstants.EtaApi_Geocoded, userModel.Location.IsGeoCoded ? "0" : "0");
//            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "1");//userModel.Location.Accuracy.ToString());
//            options.AddParm(EtaApiConstants.EtaApi_Ditance, userModel.Distance.ToString());
            
//            var api = new EtaSDKv2();
//            api.GetOfferSearch(options, 
//                q, 
//                result => {
//                    if (result != null)
//                    {
//                        Deployment.Current.Dispatcher.BeginInvoke(() => { 
//                            var byDistance = result.OrderBy(item => int.Parse(item.Store.Distance));                        Deployment.Current.Dispatcher.BeginInvoke(() => { 
//                                foreach (var item in byDistance)
//                                {
//                                    var dis = item.Store.Distance;
//                                    OffersSearch.Add(item);
//                                }
//                            });
//                        });
//                    }
//                    IsSearching = false;
//                },
//                (error,uri) =>
//                {
//                    Deployment.Current.Dispatcher.BeginInvoke(() =>
//                    {
//                        var msg = "Ups! Kunne ikke gennemfører søgning";
//#if DEBUG
//                        msg += " " + error.Message + " " + uri.ToString(); 

//#endif
//                        MessageBox.Show(msg);
//                        IsSearching = false;

//                    });
//                });
//        }

        

        //private async void InitializeModel()
        //{
        //    UserViewModel = await UserViewModel.LoadModelAsync();

        //    //UserViewModel
        //    if (UserViewModel != null && UserViewModel.Location != null && UserViewModel.Location.IsValid)
        //    {
        //        IsUserDataLoaded = true;
        //        UpdateViewModel();                
        //    }

        //}

//        public void LoadEtaCatalogList()
//        {
//            IsCatalogsUpdateing = true;
//            var userModel = UserViewModel;
//            var options = new EtaApiQueryStringParameterOptions();
//            options.AddParm(EtaApiConstants.EtaApi_Latitude, userModel.Location.Latitude.ToString("0.00000"));
//            options.AddParm(EtaApiConstants.EtaApi_Longitude, userModel.Location.Longitude.ToString("0.00000"));
//            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
//            options.AddParm(EtaApiConstants.EtaApi_Geocoded, userModel.Location.IsGeoCoded ? "0" : "0");
//            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");//userModel.Location.Accuracy.ToString());
//            options.AddParm(EtaApiConstants.EtaApi_Ditance, userModel.Distance.ToString());

//            var api = new EtaSDKv2();
//            api.GetCatalogList(options, catalogs =>
//            {
//                Deployment.Current.Dispatcher.BeginInvoke(() => {
//                    Catalogs.Clear();
//                    foreach (var catalog in catalogs)
//                    {
//                        this.Catalogs.Add(catalog);
//                    }
//                    //IsDataLoaded = true;
//                    IsCatalogsUpdateing = false;

//                });

//            }, (error,uri) => {
//                var msg = error.Message;
//                Deployment.Current.Dispatcher.BeginInvoke(() => {
//#if DEBUG
//                    MessageBox.Show(msg + "\n"+uri, "Exception", MessageBoxButton.OK);           
//#endif
//                    IsDataLoaded = false;
//                    IsCatalogsUpdateing = false;

//                });
//            });
//        }

//        public void LoadEtaStoreList()
//        {
//            IsStoresUpdateing = true;
//            var userModel = UserViewModel;
//            var options = new EtaApiQueryStringParameterOptions();
//            options.AddParm(EtaApiConstants.EtaApi_Latitude, userModel.Location.Latitude.ToString("0.00000"));
//            options.AddParm(EtaApiConstants.EtaApi_Longitude, userModel.Location.Longitude.ToString("0.00000"));
//            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
//            options.AddParm(EtaApiConstants.EtaApi_Geocoded, userModel.Location.IsGeoCoded ? "1" : "1");
//            //options.AddParm(EtaApiConstants.EtaApi_Accuracy, "10");//userModel.Location.Accuracy.ToString());
//            options.AddParm(EtaApiConstants.EtaApi_Ditance, userModel.Distance.ToString());

//            var api = new EtaSDKv2();
//            api.GetStoreList(options, stores =>
//            {
//                Deployment.Current.Dispatcher.BeginInvoke(() =>
//                {
//                    Stores.Clear();
//                    foreach (var store in stores)
//                    {
//                        this.Stores.Add(store);
//                    }
//                    //IsDataLoaded = true;
//                    IsStoresUpdateing = false;

//                });

//            }, (error, uri) =>
//            {
//                var msg = error.Message;
//                Deployment.Current.Dispatcher.BeginInvoke(() =>
//                {
//#if DEBUG
//                    MessageBox.Show(msg + "\n" + uri, "Exception", MessageBoxButton.OK);
//#endif
//                    //IsDataLoaded = false;
//                    IsStoresUpdateing = false;

//                });
//            });
//        }

        

        //internal void UpdateEtaData()
        //{
        //    this.LoadSuggestedOffers();
        //    this.LoadEtaStoreList();
        //    this.LoadEtaCatalogList();
        //    if (!string.IsNullOrWhiteSpace(OfferSearchQueryText))
        //    {
        //        this.LoadOfferSearchResult2(OfferSearchQueryText);
        //    }
        //}
        #endregion
    }
}