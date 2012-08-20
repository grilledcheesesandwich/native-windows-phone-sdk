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
using EtaSDK.ApiModels;
using System.Linq;
using System.Collections.ObjectModel;
using EtaSDK;
using EtaSDK.Utils;
using EtaSDK.v3;
using System.Threading.Tasks;

namespace EtaSampleApp.ViewModels
{
    public class StoreDetailsViewModel : ViewModelBase
    {
        private Store _store;

        public Store Store
        {
            get
            {
                return _store;
            }
            set
            {
                if (value != _store)
                {
                    _store = value;
                    NotifyPropertyChanged(() => Store);
                }
            }
        }
        //public ObservableCollection<Offer> StoreOffers { get; private set; }

        private EtaApi Api;
        public StoreDetailsViewModel(string storeId)
        {
            Api = new EtaApi();
            StoreOffers = new ObservableCollection<Offer>();
            
            Store = App.ViewModel.Stores.Where(item => item.Id == storeId).FirstOrDefault();
            if (Store != null)
            {
                LoadData();
            }
            else{
                MessageBox.Show("App.ViewModel.Stores er blevet nultillet... ØV! " + storeId);
            }
        }

        async private void LoadData()
        {
            await LoadStoreOffers();
        }

        private bool isLoading = true;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                if (value != isLoading)
                {
                    isLoading = value;

                    this.NotifyPropertyChanged(() => IsLoading);
                }
            }
        }


        #region Offers (store)
        public ObservableCollection<Offer> StoreOffers { get; private set; }

        private bool isStoreOffersLoaded = false;
        public bool IsStoreOffersLoaded
        {
            get
            {
                return isStoreOffersLoaded;
            }
            set
            {
                if (value != isStoreOffersLoaded)
                {
                    isStoreOffersLoaded = value;
                    this.NotifyPropertyChanged(() => isStoreOffersLoaded);
                    //this.NotifyPropertyChanged(() => IsDataLoaded);

                }
            }
        }

        private bool isStoreOffersLoading = false;
        public bool IsStoreOffersLoading
        {
            get
            {
                return isStoreOffersLoading;
            }
            set
            {
                if (value != isStoreOffersLoading)
                {
                    isStoreOffersLoading = value;
                    this.NotifyPropertyChanged(() => IsStoreOffersLoading);
                }
            }
        }

        async public Task LoadStoreOffers()
        {
            if (IsStoreOffersLoading)
            {
                return;
            }
            IsStoreOffersLoading = true;

            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm(EtaApiConstants.EtaApi_Latitude, App.ViewModel.UserViewModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, App.ViewModel.UserViewModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, App.ViewModel.UserViewModel.Location.IsGeoCoded ? "0" : "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");//userModel.Location.Accuracy.ToString());
            options.AddParm(EtaApiConstants.EtaApi_Ditance, "700000");
            options.AddParm("store", Store.Id);
            options.AddParm("dealer", Store.Dealer.Id);
            options.AddParm("type", "all");

            var response = await Api.GetOfferListAsync(options);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (StoreOffers.Any())
                {
                    StoreOffers.Clear();
                }
                if (response.HasErrors)
                {
                    IsStoreOffersLoaded = false;
                    HasOffers = false;
                }
                else
                {
                    foreach (var offer in response.Result)
                    {
                        StoreOffers.Add(offer);
                    }
                    IsStoreOffersLoaded = true;
                    HasOffers = true;
                }
                IsStoreOffersLoading = false;
                ShowText = !HasOffers;
            });
        }

        #endregion

        private bool hasOffers = false;
        public bool HasOffers
        {
            get
            {
                return hasOffers;
            }
            set
            {
                if (value != hasOffers)
                {
                    hasOffers = value;

                    this.NotifyPropertyChanged(() => HasOffers);
                }
            }
        }

        private bool showText = false;
        public bool ShowText
        {
            get
            {
                return showText;
            }
            set
            {
                if (value != showText)
                {
                    showText = value;

                    this.NotifyPropertyChanged(() => ShowText);
                }
            }
        }
    }
}
