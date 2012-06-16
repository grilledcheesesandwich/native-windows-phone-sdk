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
                dealer.Branding = Branding.FromJson(item["dealer"]);
                dealer.Id = item["dealer"]["id"];
                dealer.Name = item["dealer"]["name"];
                dealer.Url = item["dealer"].ContainsKey("url") && item["dealer"]["url"] != null ? item["dealer"]["url"].ToString() : "null";
                dealer.Website = item["dealer"]["website"];
                return dealer;
            }
            
            return null;
        }

    }
}
