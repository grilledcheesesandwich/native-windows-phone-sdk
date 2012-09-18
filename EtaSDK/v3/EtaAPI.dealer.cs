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
using System.Threading.Tasks;
using EtaSDK.ApiModels;
using System.Collections.Generic;
using System.Json;

namespace EtaSDK.v3
{
    public partial class EtaApi
    {
        /// <summary>
        /// Method for getting all dealer
        /// </summary>
        async public Task<EtaResponse<List<Dealer>>> GetDealers(EtaApiQueryStringParameterOptions options)
        {

            EtaApi api = new EtaApi();
            var eta = await api.ApiRawAsync("/api/v1/dealer/list/", options);
            if (eta.HasErrors)
            {
                //return new EtaResponse<List<Catalog>>(eta.Uri, eta.Error);
            }
            var jsonstr = eta.Result;
            var result = await TaskEx.Run<EtaResponse<List<Dealer>>>(() =>
            {
                if (!string.IsNullOrWhiteSpace(jsonstr))
                {
                    List<Dealer> dealers = new List<Dealer>();
                    var json = JsonValue.Parse(jsonstr);
                    foreach (var item in json["data"] as JsonArray)
                    {
                        var dealer = Dealer.FromJson(item);
                        dealers.Add(dealer);
                    }
                    return new EtaResponse<List<Dealer>>(eta.Uri, dealers);
                }
                return new EtaResponse<List<Dealer>>(eta.Uri, new Exception("json response is null or empty"));
            });
            return result;
        }
    }
}
