using System.Json;
using Esmann.WP.Common.Json;

namespace EtaSDK.ApiModels
{
    public class Weeks
    {
        public string From { get; set; }
        public string To { get; set; }

        public static Weeks FromJson(JsonValue item)
        {
            if (item.ContainsKey("weeks"))
            {
                Weeks weeks = new Weeks();
                var json = item.GetJsonValue(() => weeks);
                weeks.From = json.GetJsonValue(() => weeks.From);
                weeks.To = json.GetJsonValue(() => weeks.To);
                return weeks;
            }
            return null;
        }
    }
}
