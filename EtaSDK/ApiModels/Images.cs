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
    public class Images
    {
        public string Thumb { get; set; }
        public string View { get; set; }
        public string Zoom { get; set; }

        public static Images FromJson(JsonValue item)
        {
            if (item.ContainsKey("images"))
            {
                Images images = new Images();
                images.Thumb = item["images"]["thumb"];
                images.View = item["images"]["view"];
                images.Zoom = item["images"]["zoom"];
                return images;
            }

            return null;
        }
    }
}
