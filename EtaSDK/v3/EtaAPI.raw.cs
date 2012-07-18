using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EtaSDK.Properties;

namespace EtaSDK.v3
{
    public partial class EtaApi
    {
        private async Task<string> GetWebpage()
        {
            return await new WebClient().DownloadStringTaskAsync(new Uri("http://www.bing.com"));
        }

        private void PreConditionValidation(string resourceUri)
        {
            if (!resourceUri.StartsWith("/api/v1/"))
            {
                throw new ArgumentOutOfRangeException("resourceUri", "API reource must begin with /api/v1/");
            }
        }

        async public Task<EtaResponse<string>> ApiRawAsync(string resourceUri, EtaApiQueryStringParameterOptions options)
        {
            PreConditionValidation(resourceUri);

            var requestUri = new Uri(
                new Uri(Resources.Eta_BaseUri),
                resourceUri + options.AsQueryString());
            try
            {
                var httpClient = HttpWebRequest.CreateHttp(requestUri);
                // http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.allowreadstreambuffering(v=vs.95).aspx
                httpClient.AllowReadStreamBuffering = false; 
                httpClient.Method = options.webMethod;
                httpClient.Accept = options.responseType;

                var response = await httpClient.GetResponseAsync();
                var result = await TaskEx.Run<string>(() => {
                    string json = null;
                    if (response != null)
                    {
                        var stream = response.GetResponseStream();
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                json = reader.ReadToEnd();
                            }
                            if (json.StartsWith("<textarea>"))
                            {
                                json = json.Remove(0, 10);
                                json = json.Remove(json.Length - 11, 11);
                            }
                        }
                    }
                    return json;
                });
                return new EtaResponse<string>(result);
                
            }
            catch (Exception ex)
            {
                return new EtaResponse<string>(ex);
            }
        }

    }
}
