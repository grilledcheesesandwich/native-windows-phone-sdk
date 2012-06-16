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
using EtaSDK.Properties;
using Microsoft.Phone.Reactive;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Diagnostics;
using EtaSDK.Utils;
using System.Json;
using EtaSDK.ApiModels;

namespace EtaSDK
{
    /// <summary>
    /// Must impelment:
    /// /catalog/list/
    /// /catalog/stats/collect/ (til pageflip)
    /// /offer/list/
    /// /offer/search/
    /// /offer/info/
    /// /store/list/
    /// /store/info/
    /// </summary>
    public class EtaSDKv2
    {
        public void GetCatalogList(EtaApiQueryStringParameterOptions options, Action<List<Catalog>> callback, Action<Exception> error)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm(EtaApiConstants.EtaApi_Latitude, "55.77012");
                options.AddParm(EtaApiConstants.EtaApi_Longitude, "12.46320");
                options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
                options.AddParm(EtaApiConstants.EtaApi_Geocoded, "0");
                options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");
                options.AddParm(EtaApiConstants.EtaApi_Ditance, "10000");
            }

            ApiRaw("/api/v1/catalog/list/", options, _onresult =>
            {
                string onresult = _onresult;
                var json = JsonValue.Parse(onresult);

                List<Catalog> catalogs = new List<Catalog>();
                foreach (var item in json["data"] as JsonArray)
                {
                    var catalog = Catalog.FromJson(item);
                    catalogs.Add(catalog);
                }
                    callback(catalogs);
                
            }, (onerror, uri) =>
            {
                error (onerror);
            });
        }

        public void GetCatalogInfo(string catalogId, string publicKey, Action<Catalog> callback, Action<Exception> error)
        {
            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm("catalog", catalogId);

            ApiRaw("/api/v1/catalog/info/", options, _onresult =>
            {
                string onresult = _onresult;
                var json = JsonValue.Parse(onresult);
                var catalog = Catalog.FromJson(json);
                callback(catalog);
                
            }, (onerror, uri) =>
            {
                error (onerror);
            });
        }

        public void GetStoreInfo(EtaApiQueryStringParameterOptions options, Action<Store> callback, Action<Exception> error)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm("store", "0a63T");
            }

            ApiRaw("/api/v1/store/info/", options, onresult =>
            {
                var json = JsonValue.Parse(onresult);
                if (json.ContainsKey("data"))
                {
                    var store = Store.FromJson(json["data"], isRoot: true);
                    callback(store);
                }
                else
                {
                    callback(null);
                }

            }, (onerror, uri) =>
            {
                error(onerror);
            });
        }

        public void GetStoreList(EtaApiQueryStringParameterOptions options, Action<List<Store>> callback, Action<Exception> error)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm(EtaApiConstants.EtaApi_Latitude, "55.77012");
                options.AddParm(EtaApiConstants.EtaApi_Longitude, "12.46320");
                options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
                options.AddParm(EtaApiConstants.EtaApi_Geocoded, "0");
                options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");
                options.AddParm(EtaApiConstants.EtaApi_Ditance, "10000");
                options.AddParm("type", "all");
            }

            ApiRaw("/api/v1/store/list/", options, _onresult =>
            {
                string onresult = _onresult;
                var json = JsonValue.Parse(onresult);
                List<Store> stores = new List<Store>();
                foreach (var item in json["data"] as JsonArray)
                {
                    var store = Store.FromJson(item,isRoot: true);
                    stores.Add(store);
                }

                callback(stores);

            }, (onerror, uri) =>
            {
                error(onerror);
            });
        }

        public void GetOfferInfo(EtaApiQueryStringParameterOptions options, Action<string> callback, Action<Exception> error)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm("type", "all");
            }

            ApiRaw("/api/v1/store/list/", options, onresult =>
            {
                var json = JsonValue.Parse(onresult);

                callback(onresult);

            }, (onerror, uri) =>
            {
                error(onerror);
            });
        }

        public void GetOfferList(EtaApiQueryStringParameterOptions options, Action<string> callback, Action<Exception> error)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm("type", "all");
                options.AddParm("from",Utils.UNIXTime.GetTimestamp(DateTime.Now));
                options.AddParm("to", Utils.UNIXTime.GetTimestamp(DateTime.Now.AddDays(14)));

                options.AddParm(EtaApiConstants.EtaApi_Latitude, "55.77012");
                options.AddParm(EtaApiConstants.EtaApi_Longitude, "12.46320");
                options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
                options.AddParm(EtaApiConstants.EtaApi_Geocoded, "0");
                options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");
                options.AddParm(EtaApiConstants.EtaApi_Ditance, "10000");

            }

            ApiRaw("/api/v1/offer/list/", options, _onresult =>
            {
                string onresult = _onresult;
                var json = JsonValue.Parse(onresult);

                callback(onresult);

            }, (onerror, uri) =>
            {
                error(onerror);
            });
        }

        public void GetOfferSearch(EtaApiQueryStringParameterOptions options, string query, Action<string> callback, Action<Exception> error)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm("q", query);
            }

            ApiRaw("/api/v1/offer/search/", options, onresult =>
            {
                var json = JsonValue.Parse(onresult);

                callback(onresult);

            }, (onerror, uri) =>
            {
                error(onerror);
            });
        }

        public void GetOfferPopularSearches(Action<string> callback, Action<Exception> error)
        {
            var options = new EtaApiQueryStringParameterOptions();

            ApiRaw("/api/v1/offer/search/list/", options, onresult =>
            {
                var json = JsonValue.Parse(onresult);

                callback(onresult);

            }, (onerror, uri) =>
            {
                error(onerror);
            });
        }

        private void TESTURI(string uri)
        {
            WebClient c = new WebClient();
            c.DownloadStringAsync(new Uri(uri));
            c.DownloadStringCompleted += new DownloadStringCompletedEventHandler(c_DownloadStringCompleted);
        }

        void c_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var res = e.Result;
        }

        public void ApiRaw(string resourceUri,EtaApiQueryStringParameterOptions options, Action<string> callback, Action<Exception, Uri> error)
        {
         //   TESTURI("https://staging.etilbudsavis.dk/api/v1/catalog/info/?api_key=c2a0bb532723548de341506555749d8f&api_uuid=2ba8af5c97984a8495093da3f19e958b&api_timestamp=1338125722&catalog=2905aO&api_checksum=27303F1635DC4713268BC7698486178E");

            var requestUri = new Uri(new Uri(Resources.Eta_BaseUri), resourceUri + options.AsQueryString());

            var request = HttpWebRequest.CreateHttp( requestUri );
            request.Method = options.webMethod;
            request.Accept = options.responseType;
            
            Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                .ObserveOn(Scheduler.ThreadPool)
                .Select(response => {
                    if (response == null)
                    {
                        return null;
                    }
                    else { 
                        return response.GetResponseStream(); 
                    }
                })
                .Select(stream => {
                    if (stream == null)
                    {
                        return null; ;
                    }
                    string result;
                    using(var reader = new StreamReader(stream)){
                        result = reader.ReadToEnd();
                    }
                    return result;
                })
                .Subscribe(result => {
                    string next = result;
                    if (string.IsNullOrWhiteSpace(next))
                    {
                        error(new ArgumentNullException("result from server is null, empty or with errors"), request.RequestUri);
                        Debug.WriteLine("request rescived with errors");
                    }
                    else
                    {
                        if (next.StartsWith("<textarea>"))
                        {
                            next = next.Remove(0, 10);
                            next = next.Remove(next.Length - 11, 11);
                        }
                        callback(next);
                        Debug.WriteLine("request rescived");
                    }
                },err => {
                    var exception = err is WebException ? err as WebException : err;
                    error(exception, request.RequestUri);
                    Debug.WriteLine("request failed, uri: " + request.RequestUri.ToString());

                }, () => {
                    Debug.WriteLine("request done");
                });
        }

    }

}
