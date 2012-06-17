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

            if (item.ContainsKey("dealer"))
            {
                Dealer dealer = new Dealer();
                var json = item.GetJsonValue(() => dealer);
                dealer.Branding = Branding.FromJson(json);
                dealer.Id = json.GetJsonValue(() => dealer.Id);
                dealer.Name = json.GetJsonValue(() => dealer.Name);
                dealer.Url = json.GetJsonValue(() => dealer.Url);
                dealer.Website = json.GetJsonValue(() => dealer.Website);
                return dealer;
            }
            
            return null;
        }

    }
}
