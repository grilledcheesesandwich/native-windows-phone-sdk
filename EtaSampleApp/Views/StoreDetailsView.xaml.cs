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
using EtaSDK.ApiModels;
using EtaSampleApp.ViewModels;

namespace EtaSampleApp.Views
{
    public partial class StoreDetailsView : PhoneApplicationPage
    {
        StoreDetailsViewModel Model = null;
        public StoreDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            storeOffersListbox.SelectedIndex = -1;
            var storeId = NavigationContext.QueryString["storeId"];

            Model = new StoreDetailsViewModel(storeId);

            this.DataContext = Model;

            base.OnNavigatedTo(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // navigate to directions service/map
            NavigationService.Navigate(new Uri("/Views/NavigationView.xaml?storeId=" + Model.Store.Id, UriKind.Relative));
        }

        private void storeOffersListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            var listbox = sender as ListBox;
            if (listbox == null)
            {
                return;
            }
            if (listbox.SelectedIndex == -1)
            {
                return;
            }

            if (listbox.SelectedItem == null)
            {
                return;
            }
            var offer = listbox.SelectedItem as Offer;
            if (offer == null)
            {
                return;
            }
            App.ViewModel.SelectedOffer = offer;
            NavigationService.Navigate(new Uri("/Views/OfferView.xaml?offerId=" + offer.Id, UriKind.Relative));
        }
    }
}