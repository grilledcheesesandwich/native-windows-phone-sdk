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
using EtaSDK.Utils;
using System.Threading;
using System.IO.IsolatedStorage;

namespace EtaSampleApp.Pages
{
    public partial class CatalogPageView : PhoneApplicationPage
    {
        public CatalogPageView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string id = "3c41wO"; // føtex
            if (NavigationContext.QueryString.ContainsKey("catalogId"))
            {
                id = NavigationContext.QueryString["catalogId"];
            }
            string page = "1";
            if (NavigationContext.QueryString.ContainsKey("page"))
            {

                page = NavigationContext.QueryString["page"];
            }

            LoadZoomPage(id, page);
            
            base.OnNavigatedTo(e);
        }
        private IsoStorageHelper isoStore = new IsoStorageHelper();

        private void LoadZoomPage(string id , string page)
        {
            var path = string.Format("Catalogs/{0}/zoom/{1}.jpg", id, page);
            //var path = string.Format("Catalogs/{0}/{1}.jpg", id, page);
            if (isoStore.FileExists(path))
            {
                this.DataContext = new CatalogPageViewModel { ImagePath = path };

                return;
            }

            ThreadPool.QueueUserWorkItem(state =>
            {
                WebClient client = new WebClient();

                string uri = CatalogPageHelper.GetPageUri(id, page, ImageResolution.Zoom);
                client.OpenReadAsync(new Uri(uri));
                client.OpenReadCompleted += (sender, e) =>
                {
                    string temp = uri;
                    if (e.Error != null || e.Cancelled)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("Ups, kunne ikke hente zoom siden");
                        });
                        return;
                    }
                    using (var stream = e.Result)
                    {

                        if (isoStore.FileExists(path))
                        {
                            this.DataContext = path;
                            return;
                        }

                        using (IsolatedStorageFileStream fileStream = isoStore.CreateFile(path))
                        {
                            stream.CopyTo(fileStream);
                            fileStream.Close();
                        }
                    }
                    
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.DataContext = new CatalogPageViewModel { ImagePath = path };
                    });

                    return;

                };
            });
        }
    }

    public class CatalogPageViewModel
    {
        public string ImagePath { get; set; }
    }
}