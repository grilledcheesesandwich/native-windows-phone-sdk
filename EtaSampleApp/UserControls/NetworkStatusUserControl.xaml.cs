﻿using System;
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
using Microsoft.Phone.Tasks;

namespace EtaSampleApp.UserControls
{
    public partial class NetworkStatusUserControl : UserControl
    {
        public NetworkStatusUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConnectionSettingsTask con = new ConnectionSettingsTask();
            con.Show();
        }
    }
}
