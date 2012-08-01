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

namespace OIORestServices{
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


           
        }
        
}
