using System.Json;
using Esmann.WP.Common.Json;

namespace EtaSDK.ApiModels
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
                Store store = new Store();
                var json = isRoot ? item : item.GetJsonValue(()=>store); 

                store._RawJsonString = json.ToString();

                store.City = json.GetJsonValue(() => store.City); 
                store.Dealer = Dealer.FromJson(json);
                store.Distance = json.GetJsonValue(() => store.Distance);
                store.Id = json.GetJsonValue(() => store.Id);
                store.Latitude = json.GetJsonValue(() => store.Latitude);
                store.Longitude = json.GetJsonValue(() => store.Longitude);
                store.Street = json.GetJsonValue(() => store.Street);
                store.Zipcode = json.GetJsonValue(() => store.Zipcode); 
                store.Country = Country.FromJson(json);
                return store;
            }

            return null;
        }

    }
}
