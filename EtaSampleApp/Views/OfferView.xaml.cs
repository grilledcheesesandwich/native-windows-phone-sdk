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
    }
}