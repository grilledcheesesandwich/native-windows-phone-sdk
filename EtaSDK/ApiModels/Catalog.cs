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
using System.Json;

namespace EtaSDK.ApiModels
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
        public Images Images { get; set; }


        public string _RawJsonString{get;set;}

        public static Catalog FromJson(JsonValue item)
        {
            if(item != null){
                var catalog = new Catalog();
                catalog._RawJsonString = item.ToString();

                catalog.Id = item["id"];
                catalog.PageCount = item["pageCount"].ToString();
                catalog.Promoted = item.ContainsKey("promoted") && item["promoted"] != null ? item["promoted"].ToString() : "null";
                catalog.Expires = item["expires"].ToString();
                
                catalog.PublicKey = item["publicKey"];
                catalog.RunFrom = item["runFrom"].ToString();
                catalog.RunTill = item["runTill"].ToString();
                catalog.SelectStores = item["selectStores"].ToString();

                catalog.Week = Week.FromJson(item);

                catalog.Branding = Branding.FromJson(item);

                catalog.Dealer = Dealer.FromJson(item);
                catalog.Store = Store.FromJson(item);

                catalog.Images = Images.FromJson(item);

                return catalog;
            }
            return null;
        }

    }
}
