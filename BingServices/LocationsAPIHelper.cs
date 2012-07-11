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
using System.Threading.Tasks;
using System.Device.Location;
using BingServices.LocationsAPI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Json;

namespace BingServices
{
    public class LocationsAPIHelper
    {
        public async Task<GeocodeLocation> ZipCodeToGeoCoordinateAsync(string zipCode, string country = "Denmark")
        {
            var tcs = new TaskCompletionSource<GeocodeLocation>();

            string address = string.Format("{0}, {1}", zipCode, country);
            GeocodeRequest request = new GeocodeRequest();
            request.Credentials = new Credentials();
            request.Credentials.ApplicationId = Settings.BingmapsAPIKey;
            request.Query = address;

            var filters = new ObservableCollection<FilterBase>();
            filters.Add(new ConfidenceFilter() { MinimumConfidence = Confidence.Medium });
            GeocodeOptions options = new GeocodeOptions();
            options.Filters = filters;
            request.Options = options;


            GeocodeServiceClient geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            
            geocodeService.GeocodeCompleted += (object sender, GeocodeCompletedEventArgs e) => {
                if (e.Cancelled)
                {
                    tcs.SetCanceled();
                }
                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                if (e.Result == null || e.Result.Results.Count == 0)
                {
                    tcs.SetResult(null);
                }
                var geoCodeResult = e.Result.Results.FirstOrDefault();
                if (geoCodeResult == null)
                {
                    tcs.TrySetResult(null);
                }
                if(geoCodeResult.Locations == null || geoCodeResult.Locations.Count == 0){
                     tcs.SetResult(null);
                }
                var location = geoCodeResult.Locations.FirstOrDefault();
                if (location == null)
                {
                    tcs.SetResult(null);
                }
                tcs.SetResult(location);
            };
            geocodeService.GeocodeAsync(request);
            return await tcs.Task;
        }

        public async Task<string> GeoCoordinateToZipCodeAsync(double lat, double lon)
        {
            var tcs = new TaskCompletionSource<string>();
            WebClient client = new WebClient();
            string uri = string.Format("http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?o=json&key={2}", lat.ToString().Replace(',', '.'), lon.ToString().Replace(',', '.'), Settings.BingmapsAPIKey);
            var result = await client.DownloadStringTaskAsync(new Uri(uri));

            if (!string.IsNullOrWhiteSpace(result))
            {
                var json = JsonValue.Parse(result);
                if (json != null)
                {
                    try
                    {
                        string postal = json["resourceSets"][0]["resources"][0]["address"]["postalCode"];
                        tcs.SetResult(postal);
                        
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }
            }
            return await tcs.Task;


            //bool taskCompleted = false;

            //string point = "/51.504360719046616,-0.12600176611298197";// string.Format("userLocation={0}, {1}", lat, lon);
            //GeocodeRequest request = new GeocodeRequest();
            //request.Credentials = new Credentials();
            //request.Credentials.ApplicationId = Settings.BingmapsAPIKey;
            //request.Query = point;

            //var filters = new ObservableCollection<FilterBase>();
            //filters.Add(new ConfidenceFilter() { MinimumConfidence = Confidence.Medium });
            //GeocodeOptions options = new GeocodeOptions();
            //options.Filters = filters;
            //request.Options = options;


            //GeocodeServiceClient geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");

            //geocodeService.GeocodeCompleted += (object sender, GeocodeCompletedEventArgs e) =>
            //{
            //    if (taskCompleted)
            //    {
            //        return;
            //    }
            //    if (e.Cancelled)
            //    {
            //        tcs.SetCanceled();
            //        taskCompleted = true;
            //        return;
            //    }
            //    if (e.Error != null)
            //    {
            //        tcs.SetException(e.Error);
            //        taskCompleted = true;
            //        return;
            //    }
            //    if (e.Result == null || e.Result.Results.Count == 0)
            //    {
            //        tcs.SetResult(null);
            //        taskCompleted = true;
            //        return;
            //    }
            //    var geoCodeResult = e.Result.Results.FirstOrDefault();
            //    if (geoCodeResult == null)
            //    {
            //        tcs.SetResult(null);
            //        taskCompleted = true;
            //        return;
            //    }
            //    if (geoCodeResult.Address == null)
            //    {
            //        tcs.SetResult(null);
            //        taskCompleted = true;
            //        return;
            //    }
                
            //    tcs.SetResult(geoCodeResult.Address);
            //    taskCompleted = true;
            //};
            //geocodeService.GeocodeAsync(request);
            //return await tcs.Task;
        }
        
    }
}
