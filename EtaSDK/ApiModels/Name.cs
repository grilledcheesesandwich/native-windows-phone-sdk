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
    public class Name
    {
        public string Local { get; set; }
        public string International { get; set; }

        public static Name FromJson(JsonValue item)
        {
            if (item.ContainsKey("name"))
            {
                Name name = new Name();
                var json = item["name"];
                name.Local = json["local"];
                name.International = json["international"];
                return name;
            }
            return null;
        }
    }
}
