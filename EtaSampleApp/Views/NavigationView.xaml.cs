using System;
using System.Device.Location;
using System.Linq;
using BingServices;
using Microsoft.Phone.Controls.Maps;
using EtaSDK.ApiModels;
using Microsoft.Phone.Shell;

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
            Store store = App.ViewModel.Stores.Where(item => item.Id == storeId).FirstOrDefault();
            if (store == null)
            {
                store = App.ViewModel.SelectedStore;
            }
            if (store == null)
            {
                store = App.ViewModel.SelectedOffer.Store;
            }

            DataContext = store;

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

            RouteMap.Children.Add(new Pushpin() { Location = from, Content="o" });
            RouteMap.Children.Add(new Pushpin() { Location = to, Content=store.Dealer.Name });
        }

        private void switchView_Click(object sender, EventArgs e)
        {
            if (RouteMap.Mode is AerialMode)
            {
                RouteMap.Mode = new RoadMode();
                (sender as ApplicationBarMenuItem).Text = "Vis Luftfoto";
            }
            else
            {
                RouteMap.Mode = new AerialMode(true);
                (sender as ApplicationBarMenuItem).Text = "Vis vejkort";
            }
        }
    }
}