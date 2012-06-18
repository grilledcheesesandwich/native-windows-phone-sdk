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

namespace EtaSampleApp.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
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
    }
}