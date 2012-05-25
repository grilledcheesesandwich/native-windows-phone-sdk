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

namespace EtaSampleApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void CatalogsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var catalog = (sender as ListBox).SelectedItem as EtaSDK.Models.Catalog;
            if (catalog != null)
            {
                NavigationService.Navigate(new Uri("/Pages/CatalogDetails.xaml?catalogId=" + catalog.Id, UriKind.Relative));
            }
        }
    }
}