using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EtaSDK.Properties;

namespace EtaSDK.v3
{
    public partial class EtaApi
    {
        private void PreConditionValidation(string resourceUri, EtaApiQueryStringParameterOptions options)
        {
            if (!resourceUri.StartsWith("/api/v1/"))
            {
                throw new ArgumentOutOfRangeException("resourceUri", "API reource must begin with /api/v1/");
            }

            if (options == null)
            {
                throw new ArgumentOutOfRangeException("options", "API option is null");
            }
        }

        async public Task<EtaResponse<string>> ApiRawAsync(string resourceUri, EtaApiQueryStringParameterOptions options)
        {
            var requestUri = new Uri(
                    new Uri(Resources.Eta_BaseUri),
                    resourceUri + options.AsQueryString());
            try
            {
                PreConditionValidation(resourceUri, options);

                
                var httpClient = HttpWebRequest.CreateHttp(requestUri);
                // http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.allowreadstreambuffering(v=vs.95).aspx
                httpClient.AllowReadStreamBuffering = false; 
                httpClient.Method = options.webMethod;
                httpClient.Accept = options.responseType;

                var response = await httpClient.GetResponseAsync();
                var result = await TaskEx.Run<EtaResponse<string>>(() =>
                {
                    if (response != null)
                    {
                        var stream = response.GetResponseStream();
                        if (stream != null)
                        {
                            string json = null;
                            using (var reader = new StreamReader(stream))
                            {
                                json = reader.ReadToEnd();
                            }
                            if (json.StartsWith("<textarea>"))
                            {
                                json = json.Remove(0, 10);
                                json = json.Remove(json.Length - 11, 11);
                            }
                            return new EtaResponse<string>(requestUri,json);
                        }
                        else
                        {
                            return new EtaResponse<string>(requestUri, new Exception("ApiRawAsync: GetResponseStream is null"));
                        }
                    }
                    return new EtaResponse<string>(requestUri, new Exception("ApiRawAsync: response is null"));

                });
                return result;
            }
            catch (Exception ex)
            {
                return new EtaResponse<string>(requestUri,ex);
            }
        }

    }
}
