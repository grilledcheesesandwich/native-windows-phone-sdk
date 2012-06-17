using System.Json;
using Esmann.WP.Common.Json;

namespace EtaSDK.ApiModels
{
    public class Catalog
    {
        public string Id { get; set; }
        public string PublicKey { get; set; }
        public string Promoted { get; set; }
        public string SelectStores { get; set; }
        public string RunFrom { get; set; }
        [JsonName("runTill")]
        public string RunTill { get; set; }
        public string Expires { get; set; }
        public string PageCount { get; set; }
        
        public Weeks Week { get; set; }
        public Dealer Dealer { get; set; }
        public Store Store { get; set; }
        public Branding Branding { get; set; }
        public Images Images { get; set; }


        public string _RawJsonString{get;set;}

        public static Catalog FromJson(JsonValue json)
        {
            if(json != null){
                var catalog = new Catalog();
                catalog._RawJsonString = json.ToString();

                catalog.Id = json.GetJsonValue(()=>catalog.Id);
                catalog.PageCount = json.GetJsonValue(() => catalog.PageCount);
                catalog.Promoted = json.GetJsonValue(() => catalog.Promoted);
                catalog.Expires = json.GetJsonValue(() => catalog.Expires);

                catalog.PublicKey = json.GetJsonValue(() => catalog.PublicKey);
                catalog.RunFrom = json.GetJsonValue(() => catalog.RunFrom);
                catalog.RunTill = json.GetJsonValue(() => catalog.RunTill);
                catalog.SelectStores = json.GetJsonValue(() => catalog.SelectStores);

                catalog.Week = Weeks.FromJson(json);

                catalog.Branding = Branding.FromJson(json);

                catalog.Dealer = Dealer.FromJson(json);
                catalog.Store = Store.FromJson(json);

                catalog.Images = Images.FromJson(json);

                return catalog;
            }
            return null;
        }

    }
}
