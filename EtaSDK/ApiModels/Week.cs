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
    public class Week
    {
        public string From { get; set; }
        public string To { get; set; }

        public static Week FromJson(JsonValue item)
        {
            if (item.ContainsKey("weeks"))
            {
                Week week = new Week();
                week.From = item["weeks"]["from"].ToString();
                week.To = item["weeks"]["to"].ToString();
                return week;
            }
            return null;
        }
    }
}
