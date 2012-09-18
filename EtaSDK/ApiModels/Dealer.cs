using System.Json;
using Esmann.WP.Common.Json;

namespace EtaSDK.ApiModels
{
    public class Dealer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Website { get; set; }

        public Branding Branding { get; set; }

        public static Dealer FromJson(JsonValue item)
        {

               Dealer dealer = new Dealer();
                var json = item.ContainsKey("dealer") ? item.GetJsonValue(() => dealer) : item;
               dealer.Branding = Branding.FromJson(item);
               dealer.Id = json.GetJsonValue(() => dealer.Id);
               dealer.Name = json.GetJsonValue(() => dealer.Name);
               dealer.Url = json.GetJsonValue(() => dealer.Url);
               dealer.Website = json.GetJsonValue(() => dealer.Website);
                return dealer;
            }

    }
}
