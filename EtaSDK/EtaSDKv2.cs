﻿using System;
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
using EtaSDK.Models;

namespace EtaSDK
{
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

        public void GetCatalogInfo(string catalogId, Action<Catalog> callback, Action<Exception> error)
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
                if(json.ContainsKey("data")){
                    var store = Store.FromJson(json["data"],isRoot : true);
                    callback(store);
                }
                callback(null);

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
                //options.AddParm("type", "all");
                //options.AddParm("from",Utils.UNIXTime.GetTimestamp(DateTime.Now));
                //options.AddParm("to", Utils.UNIXTime.GetTimestamp(DateTime.Now.AddDays(14)));

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


        public void ApiRaw(string resourceUri,EtaApiQueryStringParameterOptions options, Action<string> callback, Action<Exception, Uri> error)
        {
            var requestUri = new Uri(new Uri(Resources.Eta_BaseUri), resourceUri + options.AsQueryString());

            var request = HttpWebRequest.CreateHttp( requestUri );
            request.Method = options.webMethod;
            request.Accept = options.responseType;
            
            Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                .ObserveOn(Scheduler.ThreadPool)
                .Select(response => response.GetResponseStream())
                .Select(stream => {
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
                        error(new ArgumentNullException("result from server is null or empty"), request.RequestUri);
                        Debug.WriteLine("request rescived with errors");
                    }
                    else
                    {
                        callback(next);
                        Debug.WriteLine("request rescived");
                    }
                },err => {
                    error(err, request.RequestUri);
                    Debug.WriteLine("request failed, uri: " + request.RequestUri.ToString());

                }, () => {
                    Debug.WriteLine("request done");
                });
        }

    }

}
