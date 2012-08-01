using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Device.Location;

namespace Esmann.WP.Common.ZipCodes
{
    public class GeoLocationCity
    {
        public int PostalCode { get; private set; }
        public string CityName { get; private set; }
        public GeoCoordinate Location { get; private set; }
        public bool IsUnkown { get; private set; }
        
        private GeoLocationCity()
        {

        }
        public static GeoLocationCity UnkownCity(){
            return new GeoLocationCity() { IsUnkown = true };
        }

        public GeoLocationCity(int postalCode, string cityName, double lat, double lon)
        {
            PostalCode = postalCode;
            CityName = CityName;
            Location = new GeoCoordinate(lat, lon);
        }


    }
}
