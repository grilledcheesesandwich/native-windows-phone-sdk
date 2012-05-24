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
    public class Country
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public static Country FromJson(JsonValue item)
        {
            if (item.ContainsKey("country"))
            {
                Country country = new Country();
                country.Code = item["country"]["code"];
                country.Id = item["country"]["id"].ToString();
                country.Name = item["country"]["name"];
                return country;

            }
            return null;
        }
    }
}
