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

namespace EtaSDK.Models
{
    public class Store
    {
        public string Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        
        public string Zipcode { get; set; }
        public Country Country { get; set; }
        
        public string Distance { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Dealer Dealer { get; set; }

    }
}
