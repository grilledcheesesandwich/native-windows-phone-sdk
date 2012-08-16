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
using EtaSampleApp.ViewModels;
using Esmann.WP.Common.Location;
using BingServices;

namespace EtaSampleApp.Views
{
    public partial class UserView : EtaBasePage
    {
        public UserView()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
        }

        //private void ToggleSwitchButton_Checked(object sender, RoutedEventArgs e)
        //{
        //    //App.ViewModel.UserViewModel.LoadModelAsync();// UpdateLocationInformationGPS();
        //}

        //private void ToggleSwitchButton_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    // nop
        //}

        //private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var textbox = sender as TextBox;
        //    var text = textbox.Text;
        //    if (!string.IsNullOrWhiteSpace(text))
        //    {
        //        text = text.Trim();
        //        int test = 0;
        //        if(text.Length == 4 && int.TryParse(text,out test)){
        //            if (App.ViewModel.UserViewModel.Location.ZipCode != test && !App.ViewModel.UserViewModel.Location.IsValid)
        //            {
        //                var result = await UserViewModel.GetLocationFromPostalCodeAsync(text);// UpdateLocationInformationZipCode(text);
        //                App.ViewModel.UserViewModel.Location = result;
        //            }
        //        }
        //    }
        //}
    }
}