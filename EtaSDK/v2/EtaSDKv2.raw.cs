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
    public partial class EtaSDKv2
    {

        public void ApiRaw(string resourceUri, EtaApiQueryStringParameterOptions options, Action<string> callback, Action<Exception, Uri> error)
        {
            if (!resourceUri.StartsWith("/api/v1/"))
            {
                throw new ArgumentOutOfRangeException("resourceUri","API reource must begin with /api/v1/");
            }
            var requestUri = new Uri(
                new Uri(Resources.Eta_BaseUri), 
                resourceUri + options.AsQueryString());

            var request = HttpWebRequest.CreateHttp(requestUri);
            request.Method = options.webMethod;
            request.Accept = options.responseType;

            Observable.FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                .ObserveOn(Scheduler.ThreadPool)
                .Select(response =>
                {
                    if (response == null)
                    {
                        return null;
                    }
                    else
                    {
                        return response.GetResponseStream();
                    }
                })
                .Select(stream =>
                {
                    if (stream == null)
                    {
                        return null; ;
                    }
                    string result;
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                    return result;
                })
                .Subscribe(result =>
                {
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
                }, err =>
                {
                    var exception = err is WebException ? err as WebException : err;
                    error(exception, request.RequestUri);
                    Debug.WriteLine("request failed, uri: " + request.RequestUri.ToString());

                }, () =>
                {
                    Debug.WriteLine("request done");
                });
        }

    }

}
