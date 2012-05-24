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
    public class Catalog
    {
        public string Id { get; set; }
        public string PublicKey { get; set; }
        public string Promoted { get; set; }
        public string SelectStores { get; set; }
        public string RunFrom { get; set; }
        public string RunTill { get; set; }
        public string Expires { get; set; }
        public string PageCount { get; set; }



        public Week Week { get; set; }
        public Dealer Dealer { get; set; }
        public Store Store { get; set; }
        public Branding Branding { get; set; }



    }
}
