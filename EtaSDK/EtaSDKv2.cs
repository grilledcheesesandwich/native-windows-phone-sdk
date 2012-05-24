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
    public class ApiResourceOptions
    {
        readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        public Dictionary<string, string> queryStringParams = new Dictionary<string,string>();
        public string responseType = "json";
        public string webMethod = "GET";
        public string contentType = "application/x-www-form-urlencoded";

        public ApiResourceOptions()
        {
            queryStringParams.Add(EtaApiConstants.EtaApi_ApiKey, Resources.Eta_API_Key);
            queryStringParams.Add(EtaApiConstants.EtaApi_Uuid, Uuid);
            queryStringParams.Add(EtaApiConstants.EtaApi_Timestamp, GetTimestamp(DateTime.Now));
        }

        public void AddParm(string key, string value)
        {
            if (queryStringParams.ContainsKey(key))
            {
                queryStringParams[key] = value;
            }
            else
            {
                this.queryStringParams.Add(key, value);
            }
        }

        public string GetTimestamp(DateTime time)
        {
            int seconds = (int)time.Subtract(Epoch).TotalSeconds;
            return seconds.ToString();
        }
        private string _Uuid = null; 
        public string Uuid { 
            get{
                if (_Uuid == null)
                {
                    _Uuid = UuidHelper.GetUuid();
                }
                return _Uuid;
            } 
        }

        

        public DateTime GetDateTime(string timestamp)
        {
            int seconds = Convert.ToInt32(timestamp);
            return Epoch.AddSeconds(seconds);
        }

        public string AsQueryString()
        {
            StringBuilder sb = new StringBuilder("?");
            StringBuilder checkSumValues = new StringBuilder();            
            foreach (var param in queryStringParams)
            {
                sb.Append(param.Key);
                sb.Append("=");
                sb.Append(param.Value);
                sb.Append("&");
                checkSumValues.Append(param.Value);
            }

            sb = sb.Remove(sb.Length-1,1);

            if (Resources.Eta_API_Secret != "")
            {
                checkSumValues.Append(Resources.Eta_API_Secret);

                sb.Append("&");
                sb.Append(EtaApiConstants.EtaApi_Checksum);
                sb.Append("=");
                sb.Append(MD5Core.GetHashString(checkSumValues.ToString()));
            }

            return sb.ToString();
        }
    }


    public class EtaSDKv2
    {
        public EtaSDKv2()
        {

        }

        public void GetCatalogList(ApiResourceOptions options, Action<string> callback, Action<Exception> error)
        {
            if (options == null)
            {
                options = new ApiResourceOptions();
                options.AddParm(EtaApiConstants.EtaApi_Latitude, "55.77012");
                options.AddParm(EtaApiConstants.EtaApi_Longitude, "12.46320");
                options.AddParm(EtaApiConstants.EtaApi_LocationDetermined, UNIXTime.GetTimestamp(DateTime.Now));
                options.AddParm(EtaApiConstants.EtaApi_Geocoded, "0");
                options.AddParm(EtaApiConstants.EtaApi_Accuracy, "0");
                options.AddParm(EtaApiConstants.EtaApi_Ditance, "10000");
            }

            ApiRaw("/api/v1/catalog/list/", options, onresult =>
            {
                var json = JsonValue.Parse(onresult);

                List<Catalog> catalogs = new List<Catalog>();
                foreach (var item in json["data"] as JsonArray)
                {
                    var catolog = new Catalog();

                    catolog.Id = item["id"];
                    catolog.PageCount = item["pageCount"].ToString();
                    catolog.Promoted = item.ContainsKey("promoted") && item["promoted"] != null ? item["promoted"].ToString() : null;
                    catolog.PublicKey = item["publicKey"];
                    catolog.RunFrom = item["runFrom"].ToString();
                    catolog.RunTill = item["runTill"].ToString();
                    catolog.SelectStores = item["selectStores"].ToString();
                    


                    catalogs.Add(catolog);
                }

                var s = onresult;
            }, onerror =>
            {
                error (onerror);
            });
        }

        public void ApiRaw(string resourceUri,ApiResourceOptions options, Action<string> callback, Action<Exception> error)
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
                .Subscribe(next => {
                    callback(next);
                    Debug.WriteLine("request rescived");
                },err => {
                    error(err);
                    Debug.WriteLine("request failed");

                }, () => {
                    Debug.WriteLine("request done");
                });
        }

    }

}
