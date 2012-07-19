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
using Microsoft.Phone.Shell;
using Eta.Controls;

namespace EtaSampleApp.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.ApplicationBar.IsVisible = false;
            Slider.UpdateEvent += Slider_UpdateEvent;
        }

        void Slider_UpdateEvent(object sender, SliderEventArgs e)
        {
            if (App.ViewModel.IsUserDataLoaded)
            {
                App.ViewModel.UserViewModel.Distance = e.Value;
                App.ViewModel.UpdateEtaData();

            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.DataContext = App.ViewModel;
            if (App.ViewModel.IsUserDataLoaded && !App.ViewModel.UserViewModel.FirstTimeApplicationRuns)
            {
                this.ApplicationBar.IsVisible = true;
            }
            App.ViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ViewModel_PropertyChanged);
            
            //var test = App.ViewModel.UserViewModel;
            base.OnNavigatedTo(e);
            CatalogsListBox.SelectedIndex = -1;
            StoresListBox.SelectedIndex = -1;
            searchListBox.SelectedIndex = -1;
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            App.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            base.OnNavigatedFrom(e);
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsUserDataLoaded")
            {
                if (App.ViewModel.IsUserDataLoaded)
                {
                    this.ApplicationBar.IsVisible = true;
                }
                else
                {
                    this.ApplicationBar.IsVisible = false;
                }
            }
        }

        private void LocationUserControl_Click(object sender, RoutedEventArgs e)
        {
            (sender as Control).Visibility = System.Windows.Visibility.Collapsed;
            var location = App.ViewModel.UserViewModel.Location;
            if (location.IsValid)
            {
                App.ViewModel.IsUserDataLoaded = true;
            }

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

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    searchListBox.SelectedIndex = -1;
        //    App.ViewModel.LoadOfferSearchResult(App.ViewModel.OfferSearchQueryText);
        //}

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/UserView.xaml",UriKind.Relative));
        }

        private void PhoneTextBox_ActionIconTapped(object sender, EventArgs e)
        {
            this.Focus();
            UpdateSearchList();
        }

        private void UpdateSearchList()
        {
            searchListBox.SelectedIndex = -1;
            var textbox = phoneTextBox1;
            if (App.ViewModel.OfferSearchQueryText != textbox.Text)
            {
                App.ViewModel.OfferSearchQueryText = textbox.Text;
            }
            App.ViewModel.LoadOfferSearchResult2(App.ViewModel.OfferSearchQueryText);

        }

        private void StoresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            var listbox = (sender as ListBox);
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

            var store = listbox.SelectedItem as EtaSDK.ApiModels.Store;
            if (store != null)
            {
                string uri = String.Format("/Views/StoreDetailsView.xaml?storeId={0}", store.Id);
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

        private void phoneTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
                e.Handled = true;
                UpdateSearchList();
            }

        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutView.xaml", UriKind.Relative));

        }
    }
}