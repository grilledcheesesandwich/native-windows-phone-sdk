using System.Json;
using Esmann.WP.Common.Json;

namespace EtaSDK.ApiModels
{
    public class Country
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Alpha2 { get; set; }
        //public Name Name { get; set; }

        public static Country FromJson(JsonValue item)
        {
            if (item.ContainsKey("country"))
            {
                Country country = new Country();
                var json = item.GetJsonValue(() => country);
                country.Name = json.GetJsonValue(() => country.Name);
                country.Code = json.GetJsonValue(() => country.Code);
                country.Id = json.GetJsonValue(() => country.Id);
                country.Alpha2 = json.GetJsonValue(() => country.Alpha2);

                return country;

            }
            return null;
        }
    }
}
