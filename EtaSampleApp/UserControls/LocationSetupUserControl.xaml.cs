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
using System.Windows.Controls.Primitives;

namespace EtaSampleApp.UserControls
{
    public partial class LocationSetupUserControl : UserControl
    {
        public LocationSetupUserControl()
        {
            InitializeComponent();
            btnNext.IsEnabled = false;
            DataContext = App.ViewModel;
        }

        private void ToggleSwitchButton_Checked(object sender, RoutedEventArgs e)
        {
            btnNext.IsEnabled = true;
        }

        private void ToggleSwitchButton_Unchecked(object sender, RoutedEventArgs e)
        {
            btnNext.IsEnabled = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            var text = textbox.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                text = text.Trim();
                int test = 0;
                if (text.Length == 4 && int.TryParse(text, out test))
                {
                    // Do reverse PostalCode lookup.
                    btnNext.IsEnabled = true;

                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

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

        
    }
}
