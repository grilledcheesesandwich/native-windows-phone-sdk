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
    /// /catalog/list/
    /// /catalog/stats/collect/ (til pageflip)
    /// </summary>
    public partial class EtaApi
    {
        async public Task<EtaResponse<List<Catalog>>> GetCatalogListAsync(EtaApiQueryStringParameterOptions options)
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

            var eta = await ApiRawAsync("/api/v1/catalog/list/",options);
            if (eta.HasErrors)
            {
                return new EtaResponse<List<Catalog>>(eta.Error);
            }
            var jsonstr = eta.Result;

            var result = await TaskEx.Run<EtaResponse<List<Catalog>>>(() =>
            {
                if (!string.IsNullOrWhiteSpace(jsonstr))
                {
                    List<Catalog> catalogs = new List<Catalog>();
                    var json = JsonValue.Parse(jsonstr);
                    foreach (var item in json["data"] as JsonArray)
                    {
                        var catalog = Catalog.FromJson(item);
                        catalogs.Add(catalog);
                    }
                    return new EtaResponse<List<Catalog>>(catalogs);
                }
                return new EtaResponse<List<Catalog>>(new Exception("json response is null or empty"));
            });
            return result;
        }

        async public Task<EtaResponse<Catalog>> GetCatalogInfoAsync(string catalogId, string publicKey)
        {
            var options = new EtaApiQueryStringParameterOptions();
            options.AddParm("catalog", catalogId);

            var eta = await ApiRawAsync("/api/v1/catalog/info/", options);
            if (eta.HasErrors)
            {
                return new EtaResponse<Catalog>(eta.Error);
            }

            var jsonstr = eta.Result;

            var result = await TaskEx.Run<Catalog>(() =>
            {
                Catalog catalog = null;
                if (!string.IsNullOrWhiteSpace(jsonstr))
                {
                    var json = JsonValue.Parse(jsonstr);
                    catalog = Catalog.FromJson(json);
                }
                return catalog;
            });
            return new EtaResponse<Catalog>(result);
        }

    }
}
