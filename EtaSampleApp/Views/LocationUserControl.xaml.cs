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

namespace EtaSampleApp.Views
{
    public partial class LocationUserControl : UserControl
    {
        public LocationUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler<RoutedEventArgs> Click;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
            {
                Click(this, e);
            }
        }
    }
}
