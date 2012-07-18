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
    /// /offer/list/
    /// /offer/search/
    /// /offer/info/
    /// </summary>
    public partial class EtaSDKv2
    {

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

        public void GetOfferList(EtaApiQueryStringParameterOptions options, Action<List<Offer>> callback, Action<Exception, Uri> error)
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

            ApiRaw("/api/v1/offer/list/", options, _onresult =>
            {
                string onresult = _onresult;
                var json = JsonValue.Parse(onresult);
                
                List<Offer> offers = new List<Offer>();
                foreach (var item in json["data"] as JsonArray)
                {
                    var offer = Offer.FromJson(item);
                    offers.Add(offer);
                }
                callback(offers);

            }, (onerror, uri) =>
            {
                error(onerror,uri);
            });
        }

        public void GetOfferSearch(EtaApiQueryStringParameterOptions options, string query, Action<List<Offer>> callback, Action<Exception,Uri> error)
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

            ApiRaw("/api/v1/offer/search/", options, onresult =>
            {
                var json = JsonValue.Parse(onresult);
                List<Offer> offers = new List<Offer>();
                foreach (var item in json["data"] as JsonArray)
                {
                    var offer = Offer.FromJson(item);
                    offers.Add(offer);
                }
                callback(offers);

            }, (onerror, uri) =>
            {
                error(onerror,uri);
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

    }

}
