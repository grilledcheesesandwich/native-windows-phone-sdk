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
using Esmann.WP.Common.Json;
using System.Json;
using System.Collections.Generic;
namespace EtaSDK.ApiModels
{
    public class Hotspot
    {
        /// <summary>
        /// Creates a hotspot from a json value
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Hotspot FromJson(JsonValue item)
        {
            //If the json item contains info about the polygon
            if (item.ContainsKey("poly"))
            {
                Hotspot h = new Hotspot();
                h.Polygon = new List<Point>();
                 foreach (var p in item["poly"] as JsonArray)
                {
                    h.Polygon.Add(new Point(p[0], p[1]));
                }
                h.OfferId = item["data"][0];
                return h;
            }
            return null;
        }
        /// <summary>
        /// The points of the poly
        /// </summary>
        public List<Point> Polygon { get; set; }
        /// <summary>
        /// Contains the offerid
        /// </summary>
        public string OfferId { get; set; }
    }
}
