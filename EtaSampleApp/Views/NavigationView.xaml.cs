using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BingServices;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;

namespace EtaSampleApp.Views
{
    public partial class NavigationView : EtaBasePage
    {
        public NavigationView()
        {
            InitializeComponent();
        }

        async protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            var storeId = NavigationContext.QueryString["storeId"];
            var store = App.ViewModel.Stores.Where(item => item.Id == storeId).FirstOrDefault();


            base.OnNavigatedTo(e);

            var routeApi = new RouteAPIHelper();
            var from = new GeoCoordinate 
            { 
                Latitude = App.ViewModel.UserViewModel.Location.Latitude,
                Longitude = App.ViewModel.UserViewModel.Location.Longitude 
            };
            var to = new GeoCoordinate
            {
                Latitude = double.Parse( store.Latitude),
                Longitude = double.Parse(store.Longitude)
            };
            var route = await routeApi.GetRouteAsync(from,to);
            if (route != null)
            {
                RouteLayer.Children.Add(route);
                RouteMap.SetView(LocationRect.CreateLocationRect(route.Locations));

            }


        }
    }
}