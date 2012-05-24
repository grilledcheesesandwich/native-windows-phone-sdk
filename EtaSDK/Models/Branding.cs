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
    public class Branding
    {
        public string Color { get; set; }
        public string Logo { get; set; }

        public static Branding FromJson(JsonValue item)
        {
            if (item.ContainsKey("branding"))
            {
                Branding branding = new Branding();
                branding.Color = item["branding"]["color"].ToString();
                branding.Logo = item["branding"]["logo"];
                return branding;

            }
            return null;
        }
    }
}
