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
using Microsoft.Phone.Tasks;

namespace EtaSampleApp.Views
{
    public partial class OfferView : EtaBasePage
    {
        public OfferView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.DataContext = App.ViewModel.SelectedOffer;
            base.OnNavigatedTo(e);
        }

        private void OpenCatalogButton_Click(object sender, RoutedEventArgs e)
        {
            var offer = App.ViewModel.SelectedOffer;
            if (offer ==     null)
            {
                
                return;
            }
            var catalogId = offer.Catalog.Id;
            var gotoPage = offer.Catalog.Page;
            string uri = String.Format("/Views/CatalogBrowsingView.xaml?catalogId={0}&GoToPage={1}", catalogId, gotoPage);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));

        }

        private void routeBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/NavigationView.xaml?storeId=" + App.ViewModel.SelectedOffer.Store.Id, UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            ShareLinkTask share = new ShareLinkTask();
            share.LinkUri = new Uri(App.ViewModel.SelectedOffer.Url);
            share.Title = "Tilbud på " + App.ViewModel.SelectedOffer.Heading;
            share.Message = App.ViewModel.SelectedOffer.Description.Substring(0, Math.Min(App.ViewModel.SelectedOffer.Description.Length, 40)) + "...";
            share.Show();
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            string uri = String.Format("/Views/StoreDetailsView.xaml?storeId={0}", App.ViewModel.SelectedOffer.Store.Id);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}