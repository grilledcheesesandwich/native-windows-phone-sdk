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

        private UserViewModel userViewModel = null;
        public UserViewModel UserViewModel
        {
            get
            {
                if (userViewModel == null)
                {
                    userViewModel = new UserViewModel();
                    userViewModel.LoadModelAsync();//GetUserViewModel;
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
                    });
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
    }
}