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
using System.Json;
using EtaSDK.Models;


namespace EtaSampleApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.Catalogs = new ObservableCollection<Catalog>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        public ObservableCollection<Catalog> Catalogs { get; private set; }


        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

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
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });



            //LoadEtaData();

            this.IsDataLoaded = true;

            LoadEtaCatalogList();
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
            });
        }

        private void LoadEtaData()
        {
            EtaApiOptions options = new EtaApiOptions("json","GET");
            options.AddData("api_latitude", "55.77012");
            options.AddData("api_longitude", "12.46320");
            options.AddData("api_locationDetermined", UNIXTime.GetTimestamp(DateTime.Now));
            options.AddData("api_geocoded", "0");
            options.AddData("api_accuracy", "0");
            options.AddData("api_distance", "10000");
            
            //var api = new eta.sdk.Eta("c2a0bb532723548de341506555749d8f", "80366342196ea27b14632a5b4e764b9d");
            //api.api("/api/v1/catalog/list/", options, result =>
            ////api.api("/api/v1/dealer/list/", options, result =>
            //{
            //    var res = result;

            //}, error =>
            //{
            //    var msg = (ApiState) error ;
            //    var uri = msg.request.RequestUri.ToString();
            //    var m = msg.error_message;

            //});

            EtaApiQueryStringParameterOptions options2 = new EtaApiQueryStringParameterOptions();
            options2.AddParm(EtaApiConstants.EtaApi_Latitude, "55.77012");
            options2.AddParm(EtaApiConstants.EtaApi_Longitude, "12.46320");
            options2.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
            options2.AddParm(EtaApiConstants.EtaApi_Geocoded, "0");
            options2.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");
            options2.AddParm(EtaApiConstants.EtaApi_Ditance, "10000");
            
            var api2 = new EtaSDK.EtaSDKv2();
            // done
            //api2.GetCatalogList(options2, result =>
            //{
            //    var catalogs = result;
            //    var dealerId = catalogs[0].Dealer.Id;
            //    StoreTest(dealerId);
            //}, error => {
            //    var msg = error.Message;
            //});

            // done
            //api2.GetStoreList(options2, result =>
            //{
            //    var stores = result;
            //}, error =>
            //{
            //    var msg = error.Message;
            //});

            // Offer
            //api2.GetOfferList(null, result =>
            //{
            //    var res = result;
            //}, error =>
            //{
            //    var msg = error.Message;
            //});

            api2.GetOfferPopularSearches(result =>
            {
                var res = result;
            }, error =>
            {
                var msg = error.Message;
            });

            //api2.GetOfferSearch(null,"kaffe", result =>
            //{
            //    var res = result;
            //}, error =>
            //{
            //    var msg = error.Message;
            //});
        }

        private void StoreTest(string dealerId)
        {

            EtaApiQueryStringParameterOptions options = new EtaApiQueryStringParameterOptions();
            options.AddParm("store", dealerId);

            var api2 = new EtaSDK.EtaSDKv2();
            // done
            api2.GetStoreInfo(options, result =>
            {
                var store = result;
            }, error =>
            {
                var msg = error.Message;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}