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
using EtaSDK.Utils;
using System.Collections.ObjectModel;
using EtaSampleApp.ViewModels;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.SlideView;
using System.Diagnostics;
using System.Threading;
using System.IO.IsolatedStorage;
using Esmann.WP.Common.IsolatedStorage;

namespace EtaSampleApp.Pages
{
    public partial class CatalogDetails : PhoneApplicationPage
    {
        public CatalogDetailsViewModel model;
        public CatalogDetails()
        {
            InitializeComponent();
        }
        public string Id { get; set; }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            slideView.SelectedIndex = -1;
            base.OnNavigatedTo(e);
            Id = "3c41wO"; // føtex
            if (NavigationContext.QueryString.ContainsKey("catalogId"))
            {
                Id = NavigationContext.QueryString["catalogId"];
            }
            string pageCount = "91";
            if (NavigationContext.QueryString.ContainsKey("pageCount"))
            {

                pageCount = NavigationContext.QueryString["pageCount"];
            }

            var catalog = App.ViewModel.Catalogs.Where(item => item.Id == Id).FirstOrDefault();


            LoadCatalogInfo(Id, pageCount);
            
            slideView.ManipulationCompleted += slideView_ManipulationCompleted;
        }

        // http://blogs.msdn.com/b/delay/archive/2009/03/09/controls-are-like-diapers-you-don-t-want-a-leaky-one-implementing-the-weakevent-pattern-on-silverlight-with-the-weakeventlistener-class.aspx

        void slideView_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        protected override void OnRemovedFromJournal(System.Windows.Navigation.JournalEntryRemovedEventArgs e)
        {
            model.Pages.Clear();
            model = null;
            slideView.ManipulationCompleted -= slideView_ManipulationCompleted;
            slideView.ItemTemplate = null;    // lsStuff is a RadDataBoundListBox 
            slideView = null;
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        private void LoadCatalogInfo(string id, string pageCount)
        {
            //this.slideView.TransitionMode = SlideViewTransitionMode.Flip;
            model = new CatalogDetailsViewModel(id,pageCount);
            this.DataContext = model;
        }

        void source_ItemUpdated(object sender, LoopingListDataItemEventArgs e)
        {
            //e.Item = new PanAndZoomImage();// model.Pages[e.Index];
        }

        void source_ItemNeeded(object sender, LoopingListDataItemEventArgs e)
        {
        }
        //int lastSelectedIndex = -1;
        private void CatalogListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            var listbox = (sender as ListBox);
            if(listbox == null){
                return;
            }

            if (listbox.SelectedIndex == -1)
            {
                return;
            }
            var page = listbox.SelectedItem as CatalogPage;
            if (page == null)
            {
                return;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            //string uri = String.Format("/Pages/CatalogPageView.xaml?catalogId={0}&page={1}", Id, page.Id + 1);
            //NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            slideView.Visibility = System.Windows.Visibility.Collapsed;
            zoomImage.Visibility = System.Windows.Visibility.Visible;
            LoadZoomPage(Id,page.Id);
            return;

            //if (lastSelectedIndex != -1 && lastSelectedIndex != listbox.SelectedIndex)
            //{
            //    var lastPage = model.Pages[lastSelectedIndex];
            //    lastPage.IsSelected = false;
            //    lastPage.ZoomPageUri = "";
            //}

            //if (lastSelectedIndex == listbox.SelectedIndex)
            //{
            //    return;
            //}
            
            

            //page.ZoomPageUri = page.PageUri;
            //page.IsSelected = true;
            //lastSelectedIndex = listbox.SelectedIndex;
            ////Debug.Assert(page == model.Pages[listbox.SelectedIndex], "list item is not the same!");
            ////model.Pages[listbox.SelectedIndex].ZoomPageUri = page.PageUri;
            ////model.Pages[listbox.SelectedIndex].IsSelected = true;
            ////lastSelectedIndex = listbox.SelectedIndex;

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            ////string uri = String.Format("/Pages/CatalogPageView.xaml?catalogId={0}&page={1}", Id, page.Id + 1);
            ////NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (zoomImage.Visibility == System.Windows.Visibility.Visible)
            {
                zoomImage.Visibility = System.Windows.Visibility.Collapsed;
                zoomImage.ZoomMode = ImageZoomMode.None;
                zoomImage.Source = null;
                zoomImage.ZoomMode = ImageZoomMode.Free;

                slideView.Visibility = System.Windows.Visibility.Visible;
                slideView.SelectedIndex = -1;
                e.Cancel = true;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            base.OnBackKeyPress(e);
        }

        private void LoadZoomPage(string id, int pageZeroBase)
        {
            int page = pageZeroBase + 1;
            var path = string.Format("Catalogs/{0}/zoom/{1}.jpg", id, page);
            IsoStorageHelper isoStore = new IsoStorageHelper();

            //var path = string.Format("Catalogs/{0}/{1}.jpg", id, page);
            if (isoStore.FileExists(path))
            {
                zoomImage.Source = new ImageHelper().ConvertIsoFileStreamToBitmapImage(path);
                zoomImage.Visibility = System.Windows.Visibility.Visible;
                slideView.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            ThreadPool.QueueUserWorkItem(state =>
            {
                WebClient client = new WebClient();

                string uri = CatalogPageHelper.GetPageUri(id, page.ToString(), ImageResolution.Zoom);
                client.OpenReadAsync(new Uri(uri));
                client.OpenReadCompleted += (sender, e) =>
                {
                    string temp = uri;
                    if (e.Error != null || e.Cancelled)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("Ups, kunne ikke hente zoom siden");
                            zoomImage.Source = null;
                            zoomImage.Visibility = System.Windows.Visibility.Collapsed;
                            slideView.Visibility = System.Windows.Visibility.Visible;
                        });
                        return;
                    }
                    using (var stream = e.Result)
                    {

                        if (isoStore.FileExists(path))
                        {
                            zoomImage.Source = new ImageHelper().ConvertIsoFileStreamToBitmapImage(path);
                            zoomImage.Visibility = System.Windows.Visibility.Visible;
                            slideView.Visibility = System.Windows.Visibility.Collapsed;
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
                        zoomImage.Source = new ImageHelper().ConvertIsoFileStreamToBitmapImage(path);
                        zoomImage.Visibility = System.Windows.Visibility.Visible;
                        slideView.Visibility = System.Windows.Visibility.Collapsed;
                    });

                    return;

                };
            });
        }

        
    }
}