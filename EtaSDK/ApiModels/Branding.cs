using System.Json;
using Esmann.WP.Common.Json;

namespace EtaSDK.ApiModels
{
    public class Branding
    {
        public string Color { get; set; }
        public string Logo { get; set; }

        public static Branding FromJson(JsonValue item)
        {
            if (item.ContainsKey("branding"))
            {
                Branding branding = new Branding();
                var json = item.GetJsonValue(() => branding);
                branding.Color = json.GetJsonValue(() => branding.Color);
                branding.Logo = json.GetJsonValue(() => branding.Logo);
                return branding;
            }
            return null;
        }
    }
}
