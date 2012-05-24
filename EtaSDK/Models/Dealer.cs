﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace EtaSDK.Models
{
    public class Dealer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Website { get; set; }

        public Branding Branding { get; set; }



    }
}
