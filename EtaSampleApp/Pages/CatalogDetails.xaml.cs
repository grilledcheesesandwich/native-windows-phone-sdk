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
using EtaSDK;

namespace EtaSampleApp.Pages
{
    public partial class CatalogDetails : PhoneApplicationPage
    {
        public CatalogDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string id = null;

            id = NavigationContext.QueryString["catalogId"];

            LoadCatalogInfo(id);
        }

        private void LoadCatalogInfo(string id)
        {
            var api = new EtaSDKv2();
            api.GetCatalogInfo(id, catalog =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.DataContext = catalog;

                });

            }, error =>
            {
                var msg = error.Message;
            });
        }

        private void CatalogListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}