using System.Json;
using Esmann.WP.Common.Json;


namespace EtaSDK.ApiModels
{
    public class Timespan
    {
        public string Start { get; set; }
        public string End { get; set; }

        public static Timespan FromJson(JsonValue item)
        {
            if (item.ContainsKey("timespan"))
            {
                Timespan timespan = new Timespan();
                var json = item["timespan"];
                timespan.Start = json.GetJsonValue(() => timespan.Start);
                timespan.End = json.GetJsonValue(() => timespan.End);
                return timespan;
            }
            return null;
        }
    }
}
