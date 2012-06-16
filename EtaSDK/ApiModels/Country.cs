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

namespace EtaSDK.ApiModels
{
    public class Country
    {
        //public string Id { get; set; }
        //public string Code { get; set; }
        //public string Name { get; set; }
        public string Alpha2 { get; set; }

        public Name Name { get; set; }

        public static Country FromJson(JsonValue item)
        {
            if (item.ContainsKey("country"))
            {
                Country country = new Country();
                var json = item["country"];
                country.Alpha2 = json["alpha2"];
                country.Name = Name.FromJson(json);
                //country.Code = json.ContainsKey("code") ? json["code"] : null;
                //country.sId = item["country"]["id"].ToString();
                //country.Name = item["country"]["name"];
                return country;

            }
            return null;
        }
    }
}
