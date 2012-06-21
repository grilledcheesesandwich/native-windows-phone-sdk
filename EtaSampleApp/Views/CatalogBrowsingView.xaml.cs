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
    public partial class CatalogBrowsingView : PhoneApplicationPage
    {
        private CatalogBrowsingViewModel model;
        public CatalogBrowsingView()
        {
            InitializeComponent();
            catalogSlideView.ManipulationCompleted += catalogSlideView_ManipulationCompleted;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string id = "3c41wO"; // føtex
            if (NavigationContext.QueryString.ContainsKey("catalogId"))
            {
                id = NavigationContext.QueryString["catalogId"];
            }
            
            model = new CatalogBrowsingViewModel(id);
            this.DataContext = model;
            model.LoadData();
            int startPage = 0;
            if (NavigationContext.QueryString.ContainsKey("GoToPage"))
            {
                if (int.TryParse(NavigationContext.QueryString["GoToPage"], out startPage))
                {
                    int catalogPages = 0;
                    if (int.TryParse(model.Catalog.PageCount, out catalogPages))
                    {
                        if (startPage > catalogPages)
                        {
                            startPage = 0;
                        }
                    }
                }
            }
            base.OnNavigatedTo(e);
            catalogSlideView.SelectedIndex = -1;
            ScrollToPage(startPage);
        }

        private void ScrollToPage(int startPage)
        {
            startPage = startPage - 1;
            if (startPage > 0)
            {
                catalogSlideView.UpdateLayout();
                catalogSlideView.ScrollIntoView(model.Pages[startPage]);
                catalogSlideView.UpdateLayout();
            }
        }



        private void catalogSlideView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

            var pageItem = listbox.SelectedItem as CatalogPageItem;
            if (pageItem == null)
            {
                return;
            }

            model.SelectedPageItem = pageItem;
            zoomImage.Visibility = System.Windows.Visibility.Visible;
            catalogSlideView.Visibility = System.Windows.Visibility.Collapsed;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if(zoomImage.Visibility == System.Windows.Visibility.Visible)
            {
                zoomImage.Visibility = System.Windows.Visibility.Collapsed;
                catalogSlideView.Visibility = System.Windows.Visibility.Visible;
                model.SelectedPageItem = null;
                e.Cancel = true;
            }
            base.OnBackKeyPress(e);
        }

        // GC handling!
        protected override void OnRemovedFromJournal(System.Windows.Navigation.JournalEntryRemovedEventArgs e)
        {
            model.Pages.Clear();
            model.SelectedPageItem = null;
            model = null;
            catalogSlideView.ManipulationCompleted -= catalogSlideView_ManipulationCompleted;
            catalogSlideView.ItemTemplate = null;    
            catalogSlideView = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        void catalogSlideView_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}