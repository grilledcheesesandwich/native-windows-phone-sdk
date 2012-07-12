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
        public ObservableCollection<Offer> StoreOffers { get; private set; }

        private EtaSDKv2 EtaSdk;
        public StoreDetailsViewModel(string storeId)
        {
            StoreOffers = new ObservableCollection<Offer>();
            Store = App.ViewModel.Stores.Where(item => item.Id == storeId).FirstOrDefault();
            if (Store != null)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            EtaSdk = new EtaSDK.EtaSDKv2();
            var userModel = App.ViewModel.UserViewModel;

            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm("from", EtaSDK.Utils.UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm("to", EtaSDK.Utils.UNIXTime.GetTimestamp(DateTime.Now.AddDays(14)));

            options.AddParm(EtaApiConstants.EtaApi_Latitude, userModel.Location.Latitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_Longitude, userModel.Location.Longitude.ToString("0.00000"));
            options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options.AddParm(EtaApiConstants.EtaApi_Geocoded, userModel.Location.IsGeoCoded ? "0" : "0");
            options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");//userModel.Location.Accuracy.ToString());
            options.AddParm(EtaApiConstants.EtaApi_Ditance, "700000");

            //options.AddParm(EtaApiConstants.EtaApi_OfferId, "");
            options.AddParm("store", Store.Id); // 5d6dBY
            options.AddParm("type", "suggested");


            EtaSdk.GetOfferList(options, result => {
                Deployment.Current.Dispatcher.BeginInvoke(() => {
                    StoreOffers.Clear();
                    if (result != null)
                    {
                        foreach (var offer in result.Where(item=> item.Store.Id == Store.Id))
                        {
                            StoreOffers.Add(offer);
                        }
                    }
                });
                
            
            }, (error,uri) => {

                var msg = error.Message;
            
            });
        }
    }
}
