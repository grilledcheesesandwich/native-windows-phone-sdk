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

        public string _RawJsonString { get; set; }


        public static Store FromJson(JsonValue item)
        {
            return FromJson(item, false);
        }

        public static Store FromJson(JsonValue item, bool isRoot)
        {

            if (item.ContainsKey("store") || isRoot)
            {
                var jsonStore = isRoot ? item : item["store"]; 

                Store store = new Store();
                store._RawJsonString = jsonStore.ToString();

                store.City = jsonStore["city"];
                store.Dealer = Dealer.FromJson(jsonStore);
                store.Distance = jsonStore["distance"].ToString();
                store.Id = jsonStore["id"];
                store.Latitude = jsonStore["latitude"].ToString();
                store.Longitude = jsonStore["longitude"].ToString();
                store.Street = jsonStore["street"];
                store.Zipcode = jsonStore["zipcode"].ToString();
                store.Country = Country.FromJson(jsonStore);
                return store;
            }

            return null;
        }

    }
}
