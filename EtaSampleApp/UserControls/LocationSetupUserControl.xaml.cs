using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using BingServices;
using EtaSampleApp.Strings;
using Microsoft.Phone.Tasks;

namespace EtaSampleApp.UserControls
{
    public partial class LocationSetupUserControl : UserControl
    {
        public LocationSetupUserControl()
        {
            InitializeComponent();
            btnNext.IsEnabled = false;
            DataContext = App.ViewModel;
            this.Loaded += LocationSetupUserControl_Loaded;
        }

        void LocationSetupUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var popup = this.Parent as Popup;
            if (popup == null)
            {
                btnNext.Visibility = System.Windows.Visibility.Collapsed;
                //btnCancel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void ToggleSwitchButton_Checked(object sender, RoutedEventArgs e)
        {
            btnNext.IsEnabled = true;
            App.ViewModel.UserViewModel.Location.IsGeoCoded = false;
            App.ViewModel.UserViewModel.AllowGPS = true;
        }

        private void ToggleSwitchButton_Unchecked(object sender, RoutedEventArgs e)
        {
            btnNext.IsEnabled = false;
            App.ViewModel.UserViewModel.Location.IsGeoCoded = true;
            App.ViewModel.UserViewModel.AllowGPS = false;
            TextBox_TextChanged(null,null);
        }

        async private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = postalCodeTextBox;// sender as TextBox;
            var text = textbox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                text = text.Trim();
                int test = 0;
                if (text.Length == 4 && int.TryParse(text, out test))
                {
                    progressBar.Visibility = System.Windows.Visibility.Visible;
                    // Do PostalCode to Geo Location lookup.
                    var bingApi = new LocationsAPIHelper();
                    var result = await bingApi.ZipCodeToGeoCoordinateAsync(text);
                    if (result != null)
                    {
                        App.ViewModel.UserViewModel.Location.IsGeoCoded = true;
                        App.ViewModel.UserViewModel.Location.Latitude = result.Latitude;
                        App.ViewModel.UserViewModel.Location.Longitude = result.Longitude;
                        App.ViewModel.UserViewModel.Location.IsValid = true;
                        App.ViewModel.UserViewModel.Location.Timestamp = DateTime.Now.Ticks;
                        App.ViewModel.UserViewModel.Location.ZipCode = text;

                        btnNext.IsEnabled = true;
                    }
                    progressBar.Visibility = System.Windows.Visibility.Collapsed;
                    return;
                }
            }
            if (btnNext.IsEnabled) { btnNext.IsEnabled = false; }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // not supported
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void ClosePopup()
        {
            var popup = this.Parent as Popup;
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri(AppResources.PrivacyPolicyLocationDataUri);
            browser.Show();
        }

        private void postalCodeTextBox_Tap(object sender, GestureEventArgs e)
        {
            //scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            //(sender as TextBox).Focus();
            //scrollView.ScrollToVerticalOffset(-500);

        }
    }
}
