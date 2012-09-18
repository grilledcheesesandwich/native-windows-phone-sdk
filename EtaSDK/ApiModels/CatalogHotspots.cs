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
using System.Collections.Generic;
using System.Json;
using Esmann.WP.Common.Json;
using System.Linq;
namespace EtaSDK.ApiModels
{
    public class CatalogHotspot
    {
        /// <summary>
        /// Creates a cataloghotspot from a json value
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static CatalogHotspot FromJson(JsonValue item)
        {

                CatalogHotspot ch = new CatalogHotspot();
                var dimensions = item.GetJsonValue(() => ch.Dimensions);
                ch.Dimensions = new Point(dimensions["width"], dimensions["height"]);
                foreach (var hotSpot in item["hotspots"] as JsonArray)
                {
                    ch.Hotspots.Add(Hotspot.FromJson(hotSpot));
                }
                return ch;
          
        }
        public Point Dimensions { get; set; }
        private List<Hotspot> hotspots = new List<Hotspot>();

        /// <summary>
        /// Returns a list of hotspots
        /// </summary>
        public List<Hotspot> Hotspots
        {
            get { return hotspots; }
        }
        private List<Hotspot> phoneHotspots = new List<Hotspot>();

        /// <summary>
        /// Returns a list of hotspots
        /// </summary>
        public List<Hotspot> PhoneHotspots
        {
            get { return phoneHotspots; }
        }

        /// <summary>
        /// The image dimensions on the phone
        /// </summary>
        public Point PhoneDimensions { get; set; }
        /// <summary>
        /// Add info about phone dimensions and creates a resized list of info
        /// </summary>
        /// <param name="phoneDimensions"></param>
        public void AddPhoneDimensions(Point phoneDimensions)
        {
            PhoneDimensions= phoneDimensions;
            foreach (var item in Hotspots)
            {
                //Calculate relationsship between original and new image
                double xModifier = phoneDimensions.X / Dimensions.X;
                double yModifier = phoneDimensions.Y / Dimensions.Y;
                Hotspot h = new Hotspot();
                h.OfferId = item.OfferId;
                h.Polygon= new List<Point>();
                //resize polygon to the page
                /*var minX = item.Polygon.Min(p => p.X * xModifier);
                var maxX = item.Polygon.Max(p => p.X * xModifier);
                var minY = item.Polygon.Min(p => (PhoneDimensions.Y - (p.Y * yModifier)));
                var maxY = item.Polygon.Max(p => (PhoneDimensions.Y - (p.Y * yModifier)));
                
                 */ //Create a rectange 

                foreach (var p in item.Polygon)
                {
                    h.Polygon.Add(new Point(p.X * xModifier, (PhoneDimensions.Y - (p.Y * yModifier))));
                }
                PhoneHotspots.Add(h);
	        }
        }
        /// <summary>
        /// attempts to find a matching hotspot based on the price that was clicked on
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Hotspot FindMatchingHotspot(Point p)
        {
            foreach (var h in PhoneHotspots)
            {
                if (PointInPolygon(p, h.Polygon))
                {
                    return h;
                }
            }
            return null;
        }
       /// <summary>  
    ///     Determines if the specified <see cref="PointF"/> if within this polygon.  
    /// </summary>  
    /// <remarks>  
    ///     This algorithm is extremely fast, which makes it appropriate for use in brute force algorithms.  
    /// </remarks>  
    /// <param name="point">  
    ///     The point containing the x,y coordinates to check.  
    /// </param>  
  /// <returns>  
    ///     <c>true</c> if the point is within the polygon, otherwise <c>false</c>  
    /// </returns>  
    public bool PointInPolygon(Point point, List<Point> points)  
    {  
        var j = points.Count - 1;  
        var oddNodes = false;

        for (var i = 0; i < points.Count; i++)  
        {  
            if (points[i].Y < point.Y && points[j].Y >= point.Y ||  
                points[j].Y < point.Y && points[i].Y >= point.Y)  
            {  
                if (points[i].X +  
                    (point.Y - points[i].Y)/(points[j].Y - points[i].Y)*(points[j].X - points[i].X) < point.X)  
                {  
                    oddNodes = !oddNodes;  
                }  
            }  
            j = i;  
        }  
  
        return oddNodes;  
    }  

        /*
        /// <summary>
        /// Returns true if a point is contained within the defined poly.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool IsInPolygon(Point p, List<Point> poly)
        {
            Point p1, p2;


            bool inside = false;


            if (poly.Count < 3)
            {
                return inside;
            }


            var oldPoint = new Point(
                poly[poly.Count - 1].X, poly[poly.Count - 1].Y);


            for (int i = 0; i < poly.Count; i++)
            {
                var newPoint = new Point(poly[i].X, poly[i].Y);


                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;

                    p2 = newPoint;
                }

                else
                {
                    p1 = newPoint;

                    p2 = oldPoint;
                }


                if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                    && (p.Y - (long)p1.Y) * (p2.X - p1.X)
                    < (p2.Y - (long)p1.Y) * (p.X - p1.X))
                {
                    inside = !inside;
                }


                oldPoint = newPoint;
            }


            return inside;
        }
        */
    }
}
