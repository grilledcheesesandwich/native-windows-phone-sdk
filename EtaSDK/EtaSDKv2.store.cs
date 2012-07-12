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
    /// /store/list/
    /// /store/info/
    /// </summary>
    public partial class EtaSDKv2
    {
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

        public void GetStoreList(EtaApiQueryStringParameterOptions options, Action<List<Store>> callback, Action<Exception,Uri> error)
        {
            // Offer Id el. Catalog id med som key
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm(EtaApiConstants.EtaApi_Latitude, "55.77012");
                options.AddParm(EtaApiConstants.EtaApi_Longitude, "12.46320");
                options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
                options.AddParm(EtaApiConstants.EtaApi_Geocoded, "0");
                options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");
                options.AddParm(EtaApiConstants.EtaApi_Ditance, "10000");
                //options.AddParm(EtaApiConstants.EtaApi_OfferId, "");
                //options.AddParm(EtaApiConstants.EtaApi_CatalogId, "");
                options.AddParm("type", "suggested");
            }

            ApiRaw("/api/v1/store/list/", options, _onresult =>
            {
                string onresult = _onresult;
                var json = JsonValue.Parse(onresult);
                List<Store> stores = new List<Store>();
                foreach (var item in json["data"] as JsonArray)
                {
                    var store = Store.FromJson(item, isRoot: true);
                    stores.Add(store);
                }

                callback(stores);

            }, (onerror, uri) =>
            {
                error(onerror,uri);
            });
        }
    }
}
