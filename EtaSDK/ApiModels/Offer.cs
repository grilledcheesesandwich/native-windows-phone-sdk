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
using Esmann.WP.Common.Json;


namespace EtaSDK.ApiModels
{
    public class Offer
    {
        public string Id { get; set; }
        public string SelectStores { get; set; }
        public string Promoted { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Preprice { get; set; }

        public Images Images { get; set; }

        public string Url { get; set; }

        public Dealer Dealer { get; set; }

        //public Timespan Timespan { get; set; }
        public string RunFrom { get; set; }
        public string RunTill { get; set; }
        public CatalogIndex Catalog { get; set; }
        public Store Store { get; set; }



        public static Offer FromJson(JsonValue item)
        {
            if (item != null && item.ContainsKey("id"))
            {
                Offer offer = new Offer();
                var json = item;
                offer.Id = json.GetJsonValue(() => offer.Id);
                offer.SelectStores = json.GetJsonValue(() => offer.SelectStores);
                offer.Promoted = json.GetJsonValue(() => offer.Promoted);
                offer.Heading = json.GetJsonValue(() => offer.Heading);
                offer.Description = json.GetJsonValue(() => offer.Description);
                offer.Price = json.GetJsonValue(() => offer.Price);
                offer.Preprice = json.GetJsonValue(() => offer.Preprice);
                offer.Images =  Images.FromJson(json);
                offer.Url = json.GetJsonValue(() => offer.Url);
                offer.RunFrom = json.GetJsonValue(() => offer.RunFrom);
                offer.RunTill = json.GetJsonValue(() => offer.RunTill);
                offer.Dealer = Dealer.FromJson(json);

                //offer.Timespan = Timespan.FromJson(json.GetJsonValue(() => offer.Timespan));
                
                offer.Catalog = CatalogIndex.FromJson(json);
                offer.Store = Store.FromJson(json);


                return offer;
            }
            return null;
        }
    }
}
