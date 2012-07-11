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
    /// </summary>
    public partial class EtaSDKv2
    {
        public void GetCatalogList(EtaApiQueryStringParameterOptions options, Action<List<Catalog>> callback, Action<Exception, Uri> error)
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
                if (!string.IsNullOrWhiteSpace(onresult))
                {
                    try
                    {
                        var json = JsonValue.Parse(onresult);

                        List<Catalog> catalogs = new List<Catalog>();
                        foreach (var item in json["data"] as JsonArray)
                        {
                            var catalog = Catalog.FromJson(item);
                            catalogs.Add(catalog);
                        }
                        callback(catalogs);
                    }
                    catch (Exception ex)
                    {
                        error(new ArgumentNullException("Ups ingen data fra serveren", ex), null);
                    }
                }
                else
                {
                    error(new ArgumentNullException("Ups ingen data fra serveren"), new Uri(""));
                }

            }, (onerror, uri) =>
            {
                error(onerror, uri);
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
                error(onerror);
            });
        }

    }
}
