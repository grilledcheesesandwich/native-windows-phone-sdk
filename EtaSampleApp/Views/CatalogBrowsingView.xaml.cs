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


// list box help:
// http://blogs.msdn.com/b/ptorr/archive/2010/10/12/procrastination-ftw-lazylistbox-should-improve-your-scrolling-performance-and-responsiveness.aspx

namespace EtaSampleApp.Views
{
    public partial class CatalogBrowsingView : EtaBasePage
    {
        public static SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            return new SolidColorBrush(
                Color.FromArgb(
                    255,
                    Convert.ToByte(hexaColor.Substring(0, 2), 16),
                    Convert.ToByte(hexaColor.Substring(2, 2), 16),
                    Convert.ToByte(hexaColor.Substring(4, 2), 16)
                    //Convert.ToByte(hexaColor.Substring(6, 2), 16)
                    //Convert.ToByte(hexaColor.Substring(1, 2), 16),
                    //Convert.ToByte(hexaColor.Substring(3, 2), 16),
                    //Convert.ToByte(hexaColor.Substring(5, 2), 16),
                    //Convert.ToByte(hexaColor.Substring(7, 2), 16)
                )
            );
        }

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
            
            
            if (!string.IsNullOrWhiteSpace(model.Catalog.Branding.Color))
            {
                //model.Catalog.Week.From
                //this.ContentPanel.Background = new SolidColorBrush(Colors.Red);
                this.ContentPanel.Background = GetColorFromHexa(model.Catalog.Branding.Color);
            }

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
                slider.Value = startPage;
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
            slider.Value = listbox.SelectedIndex;

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
            var list = VisualTreeHelper.FindElementsInHostCoordinates(new Rect(new Point(0, 0), new Point(800, 480)), catalogSlideView).OfType<ListBoxItem>();
            if (list.Any())
            {
                CatalogPageItem ListBoxItemInFocus = null;
                
                var count = list.Count();
                if (count == 1)
                {
                    ListBoxItemInFocus = list.First().Content as CatalogPageItem;
                }
                else
                {
                    int value = (int)Math.Ceiling(count / 2d);
                     ListBoxItemInFocus = list.ToList()[value].Content as CatalogPageItem;

                }
                model.CurrentPage = ListBoxItemInFocus.Id;
            }
           
            //foreach (var item in catalogSlideView.Items)
            //{
            //    var isVisible = TestVisibility(item, catalogSlideView, System.Windows.Controls.Orientation.Vertical, true);
            //    if (isVisible)
            //    {
            //        int bbbb = 0;
            //    }
            //}
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //var slider = sender as Slider;
            if (catalogSlideView != null && catalogSlideView.SelectedIndex != -1 && catalogSlideView.SelectedIndex != e.NewValue)
            {
                ScrollToPage((int)e.NewValue);
            };

        }

        int currentPageIndex = 0;
        private void catalogSlideView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        public bool TestVisibility(FrameworkElement item, FrameworkElement viewport, Orientation orientation, bool wantVisible)
        {
            // Determine the bounding box of the item relative to the viewport 
            GeneralTransform transform = item.TransformToVisual(viewport);
            Point topLeft = transform.Transform(new Point(0, 0));
            Point bottomRight = transform.Transform(new Point(item.ActualWidth, item.ActualHeight));

            // Check for overlapping bounding box of the item vs. the viewport, depending on orientation 
            double min, max, testMin, testMax;
            if (orientation == System.Windows.Controls.Orientation.Vertical)
            {
                min = topLeft.Y;
                max = bottomRight.Y;
                testMin = 0;
                testMax = Math.Min(viewport.ActualHeight, double.IsNaN(viewport.Height) ? double.PositiveInfinity : viewport.Height);
            }
            else
            {
                min = topLeft.X;
                max = bottomRight.X;
                testMin = 0;
                testMax = Math.Min(viewport.ActualWidth, double.IsNaN(viewport.Width) ? double.PositiveInfinity : viewport.Width);
            }

            bool result = wantVisible;

            if (min >= testMax || max <= testMin)
                result = !wantVisible;

            return result;
        }

    }
}