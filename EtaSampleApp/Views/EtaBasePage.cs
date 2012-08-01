using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EtaSampleApp.Views
{
    public class EtaBasePage : PhoneApplicationPage
    {
        public static SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            int offset = hexaColor.StartsWith("#") ? 1 : 0;
            bool alpha = offset == 1 && hexaColor.Length == 9 || offset == 0 && hexaColor.Length == 8;
            byte alphaValue = 255;
            if (alpha)
            {
                alphaValue = Convert.ToByte(hexaColor.Substring(offset, 2), 16);
            }
            return new SolidColorBrush(
                Color.FromArgb(
                    alphaValue,
                    Convert.ToByte(hexaColor.Substring(0 + offset + (alpha ? 2 : 0), 2), 16),
                    Convert.ToByte(hexaColor.Substring(2 + offset + (alpha ? 2 : 0), 2), 16),
                    Convert.ToByte(hexaColor.Substring(4 + offset + (alpha ? 2 : 0), 2), 16)
                    ));
        }

        PerformanceProgressBar progressIndicator;

        public EtaBasePage()
            : base()
        {
            this.Background = GetColorFromHexa("#9CB227");
            //progressIndicator = new PerformanceProgressBar() 
            //    { 
            //        Foreground = new SolidColorBrush( Colors.White),
            //        Background = new SolidColorBrush(Colors.Transparent)
            //        Width = 480,
            //        Height = 10,
            //        IsIndeterminate = true
            //    };
            


        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
           
            base.OnNavigatedTo(e);
            SystemTray.IsVisible = true;
            SystemTray.Opacity = 0;
            SystemTray.ForegroundColor = GetColorFromHexa("FFFFFE").Color;
            SystemTray.BackgroundColor = GetColorFromHexa("FFFFFE").Color;
            
            ProgressIndicator progressIndicator = new ProgressIndicator(); 
            progressIndicator.IsVisible = true; 
            progressIndicator.IsIndeterminate = true;
            
            progressIndicator.Text = "Loading...";

            SystemTray.SetProgressIndicator(this, progressIndicator);
        }
    }
}
