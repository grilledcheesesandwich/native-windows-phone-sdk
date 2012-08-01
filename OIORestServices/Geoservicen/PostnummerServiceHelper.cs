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
using System.Json;

namespace OIORestServices.Geoservicen
{
    public class PostnummerServiceHelper
    {
        public async Task<string> GeoToPostalCode(double lat, double lon)
        {
            var tcs = new TaskCompletionSource<string>();
            WebClient client = new WebClient();
            string uri = string.Format("http://geo.oiorest.dk/postnumre/{0},{1}.json", lat.ToString().Replace(',', '.'), lon.ToString().Replace(',', '.'));
            var result = await client.DownloadStringTaskAsync(new Uri(uri));

            if (!string.IsNullOrWhiteSpace(result))
            {
                var json = JsonValue.Parse(result);
                if (json != null)
                {
                    try
                    {
                        string postal = json["fra"];
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

        public async Task<string> PostalCodeToGeo(string postalCode)
        {
            double lat =0, lon =0;
            var tcs = new TaskCompletionSource<string>();
            WebClient client = new WebClient();
            string uri = string.Format("http://geo.oiorest.dk/postnumre/{0},{1}.json", lat.ToString().Replace(',', '.'), lon.ToString().Replace(',', '.'));
            var result = await client.DownloadStringTaskAsync(new Uri(uri));

            if (!string.IsNullOrWhiteSpace(result))
            {
                var json = JsonValue.Parse(result);
                if (json != null)
                {
                    try
                    {
                        string postal = json["fra"];
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
}
