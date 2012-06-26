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
using EtaSDK;
using EtaSDK.Utils;
using System.Globalization;

namespace EtaSampleApp.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            if (!App.ViewModel.UserViewModel.FirstTimeApplicationRuns)
            {
                locationControl.Visibility = System.Windows.Visibility.Collapsed;
            }
           
        }

       
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.DataContext = App.ViewModel;
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            base.OnNavigatedTo(e);
            CatalogsListBox.SelectedIndex = -1;
            searchListBox.SelectedIndex = -1;
        }

        private void LocationUserControl_Click(object sender, RoutedEventArgs e)
        {
            (sender as Control).Visibility = System.Windows.Visibility.Collapsed;
        }

        private void CatalogsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender == null){
                return;
            }

            var listbox =  (sender as ListBox);
            if(listbox == null){
                return;
            }

            if(listbox.SelectedIndex == -1){
                return;
            }

            if (listbox.SelectedItem == null)
            {
                return;
            }

            var catalog = listbox.SelectedItem as EtaSDK.ApiModels.Catalog;
            if (catalog != null)
            {
                string uri = String.Format("/Views/CatalogBrowsingView.xaml?catalogId={0}", catalog.Id);
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

        private void searchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            NavigationService.Navigate(new Uri("/Views/OfferView.xaml?offerId=" + offer.Id,UriKind.Relative));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            searchListBox.SelectedIndex = -1;
            App.ViewModel.LoadOfferSearchResult(App.ViewModel.OfferSearchQueryText);
        }

        

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/UserView.xaml",UriKind.Relative));
        }
    }
}