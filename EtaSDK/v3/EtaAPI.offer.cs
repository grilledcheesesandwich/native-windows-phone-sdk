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
    /// /offer/list/
    /// /offer/search/
    /// /offer/info/
    /// </summary>
    public partial class EtaApi
    {

        async public Task<EtaResponse<JsonValue>> GetOfferInfoAsync(EtaApiQueryStringParameterOptions options)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm("type", "all");
            }

            var eta = await ApiRawAsync("/api/v1/offer/info/", options);
            if (eta.HasErrors)
            {
                return new EtaResponse<JsonValue>(eta.Uri, eta.Error);
            }

            var jsonstr = eta.Result;

            var result = await TaskEx.Run<EtaResponse<JsonValue>>(() =>
            {
                if (!string.IsNullOrWhiteSpace(jsonstr))
                {
                    var json = JsonValue.Parse(jsonstr);
                    return new EtaResponse<JsonValue>(eta.Uri,json);
                }
                return new EtaResponse<JsonValue>(eta.Uri, new Exception("Jsonstr value is null or empty"));
            });
            return result;
        }

        async public Task<EtaResponse<List<Offer>>> GetOfferListAsync(EtaApiQueryStringParameterOptions options)
        {
            if (options == null)
            {
                options = new EtaApiQueryStringParameterOptions();
                options.AddParm("from", EtaSDK.Utils.UNIXTime.GetTimestamp(DateTime.Now));
                options.AddParm("to", EtaSDK.Utils.UNIXTime.GetTimestamp(DateTime.Now.AddDays(14)));

                options.AddParm(EtaApiConstants.EtaApi_Latitude, "55.77012");
                options.AddParm(EtaApiConstants.EtaApi_Longitude, "12.46320");
                options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
                options.AddParm(EtaApiConstants.EtaApi_Geocoded, "0");
                options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");
                options.AddParm(EtaApiConstants.EtaApi_Ditance, "10000");

                //options.AddParm(EtaApiConstants.EtaApi_OfferId, "");
                options.AddParm("store", "5d6dBY"); // 5d6dBY
                options.AddParm("type", "suggested");
            }

            var eta = await ApiRawAsync("/api/v1/offer/list/", options);
            if (eta.HasErrors)
            {
                return new EtaResponse<List<Offer>>(eta.Uri, eta.Error);
            }

            var jsonstr = eta.Result;

            var result = await TaskEx.Run<EtaResponse<List<Offer>>>(() =>
            {
                return ParseOffers(eta.Uri, jsonstr);
            });
            return result;
        }

        async public Task<EtaResponse<List<Offer>>> GetOfferSearchAsync(EtaApiQueryStringParameterOptions options, string query)
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

            options.AddParm("q", query);

            var eta = await ApiRawAsync("/api/v1/offer/search/", options);
            if (eta.HasErrors)
            {
                return new EtaResponse<List<Offer>>(eta.Uri, eta.Error);
            }
            var jsonstr = eta.Result;

            var result = await TaskEx.Run <EtaResponse<List<Offer>>>(() =>
            {
                return ParseOffers(eta.Uri, jsonstr);
            });
            return result;
        }

        //public void GetOfferPopularSearches(Action<string> callback, Action<Exception> error)
        //{
        //    var options = new EtaApiQueryStringParameterOptions();

        //    ApiRaw("/api/v1/offer/search/list/", options, onresult =>
        //    {
        //        var json = JsonValue.Parse(onresult);

        //        callback(onresult);

        //    }, (onerror, uri) =>
        //    {
        //        error(onerror);
        //    });
        //}

        private EtaResponse<List<Offer>> ParseOffers(Uri uri,string jsonstr)
        {
            try
            {
                var json = JsonValue.Parse(jsonstr);
                List<Offer> offers = new List<Offer>();
                foreach (var item in json["data"] as JsonArray)
                {
                    var offer = Offer.FromJson(item);
                    offers.Add(offer);
                }
                return new EtaResponse<List<Offer>>(uri, offers);
            }
            catch (Exception ex)
            {
                return new EtaResponse<List<Offer>>(uri, ex);
            }
        }

    }

}
