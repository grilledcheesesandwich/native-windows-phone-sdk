using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using EtaSDK.ApiModels;
using EtaSDK.Utils;

namespace EtaSDK.v3
{
    /// <summary>
    /// Must impelment:
    /// /store/list/
    /// /store/info/
    /// </summary>
    public partial class EtaApi
    {
        async public Task<EtaResponse<Store>> GetStoreInfoAsync(EtaApiQueryStringParameterOptions options)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm("store", "0a63T");
            }

            var eta = await ApiRawAsync("/api/v1/store/info/", options);
            if (eta.HasErrors)
            {
                return new EtaResponse<Store>(eta.Uri, eta.Error);
            }
            var jsonstr = eta.Result;
            var result = await TaskEx.Run<EtaResponse<Store>>(() =>
            {
                if (!string.IsNullOrWhiteSpace(jsonstr))
                {
                    var json = JsonValue.Parse(jsonstr);
                    var store = Store.FromJson(json["data"], isRoot: true);
                    return new EtaResponse<Store>(eta.Uri, store);
                }
                return new EtaResponse<Store>(eta.Uri, new Exception("response is null or empty"));
            });
            return result;
        }

        async public Task<EtaResponse<List<Store>>> GetStoreListAsync(EtaApiQueryStringParameterOptions options)
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

            var eta = await ApiRawAsync("/api/v1/store/list/", options);
            if (eta.HasErrors)
            {
                return new EtaResponse<List<Store>>(eta.Uri, eta.Error);
            }
            var jsonstr = eta.Result;

            var result = await TaskEx.Run<EtaResponse<List<Store>>>(() =>
            {
                if (!string.IsNullOrWhiteSpace(jsonstr))
                {
                    List<Store> stores = new List<Store>();
                    var json = JsonValue.Parse(jsonstr);
                    foreach (var item in json["data"] as JsonArray)
                    {
                        var store = Store.FromJson(item, isRoot: true);
                        stores.Add(store);
                    }
                    return new EtaResponse<List<Store>>(eta.Uri, stores);
                }
                return new EtaResponse<List<Store>>(eta.Uri, new Exception("JsonValue is null or empty"));
            });
            return result;
        }
    }
}
