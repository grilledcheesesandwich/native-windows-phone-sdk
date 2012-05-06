using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Info;
using Microsoft.Phone.Reactive;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace eta.sdk
{
    public struct ApiState
    {
        public HttpWebRequest request;
        public string sdata;
        public EtaApiOptions options;
        public Action<object> success, error;
        public string error_message;
        public HttpWebResponse response;
        public string response_body;
    }

    public class Eta
    {
        const int cacheExpiration = 10 * 60 * 1000;

        public string apiKey;
        public string apiSecret;
        public string uuid;

        internal const string domain = "http://web-1540031923.eu-west-1.elb.amazonaws.com"; // Todo: etilbudsavis.dk
        public Dictionary<string, string> accepts;

        /// <summary>
        /// Constructor, replaces "init" from js.
        /// </summary>
        /// <param name="apiKey">This app's key</param>
        /// <param name="apiSecret">This app's secret</param>
        public Eta(string apiKey, string apiSecret = "")
        {
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
            this.uuid = this.GetUuid();


            this.accepts = new Dictionary<string, string>();
            this.accepts.Add("xml", "application/xml, text/xml");
            this.accepts.Add("html", "text/html");
            this.accepts.Add("text", "text/plain");
            this.accepts.Add("json", "application/json, text/javascript");

        }

        /// <summary>
        /// The API call
        /// </summary>
        /// <param name="url">Url to call, must start with "/api/"</param>
        /// <param name="options">An EtaApiObjects instance with any data to send</param>
        /// <param name="success">A method taking a ApiState instance to be called on success.</param>
        /// <param name="error">A method taking a ApiState instance to be called on success.</param>
        public void api(string url, EtaApiOptions options, Action<object> success, Action<object> error)
        {
            List<KeyValuePair<string, string>> param = this.FormatParams(options.data);
            string data = this.BuildParams(param);

            if (!url.StartsWith("/api/"))
            {
                throw new EtaException("Illegal URL!");
            }

            options.type = options.type.ToUpper();

            ApiState state = new ApiState();

            string target = Eta.domain + url;

            if (options.type == "GET")
            {
                target += "?" + data;
                state.sdata = "";
            }
            else
            {
                state.sdata = data;
            }

            HttpWebRequest xhr = (HttpWebRequest)System.Net.HttpWebRequest.Create(target);

            xhr.Accept = this.accepts[options.dataType];
            xhr.Method = options.type;

            state.request = xhr;
            state.options = options;
            state.success = success;
            state.error = error;

            Debug.WriteLine(target);

            try
            {
                if (options.type == "POST")
                {
                    xhr.ContentType = options.contentType;
                    xhr.BeginGetRequestStream(new AsyncCallback(Eta.RequestOpen), state);
                }
                else
                {
                    xhr.BeginGetResponse(new AsyncCallback(Eta.ResponseOpen), state);
                }
            }
            catch (Exception e)
            {
                throw new EtaException("Could not open connection.\n" + e.Message);
            }
        }

        /// <summary>
        /// Request open callback, for post requests to send data.
        /// </summary>
        /// <param name="result">The async result, internally generated.</param>
        private static void RequestOpen(IAsyncResult result)
        {
            ApiState state = (ApiState)result.AsyncState;

            Stream ostream = null;
            try
            {
                ostream = state.request.EndGetRequestStream(result);
            }
            catch (WebException e)
            {
                state.error_message = e.Message;
                state.response = (HttpWebResponse)e.Response;
                state.error(state);
                return;
            }

            if (state.sdata.Length > 0)
            {
                byte[] sendbuffer = System.Text.Encoding.UTF8.GetBytes(state.sdata);
                ostream.Write(sendbuffer, 0, sendbuffer.GetLength(0));
            }
            ostream.Close();

            state.request.BeginGetResponse(new AsyncCallback(Eta.ResponseOpen), state);
        }

        /// <summary>
        /// Get web response to read from server.
        /// </summary>
        /// <param name="result">The async result, internally generated.</param>
        private static void ResponseOpen(IAsyncResult result)
        {
            ApiState state = (ApiState)result.AsyncState;

            WebResponse response;
            try
            {
                response = state.request.EndGetResponse(result);
            }
            catch (WebException e)
            {
                state.error_message = e.Message;
                state.response = (HttpWebResponse)e.Response;
                state.error(state);
                return;
            }

            state.response = (HttpWebResponse)response;

            Stream istream = response.GetResponseStream();
            int length = (int)istream.Length;
            byte[] buffer = new byte[length];
            istream.Read(buffer, 0, length);

            state.response_body = Encoding.UTF8.GetString(buffer, 0, length);

            state.success(state);
        }
        
        /// <summary>
        /// Called to format the data parameters from a dictionary to a list of kvps.
        /// Also prepends the constant api data. This is needed to keep the list sorted.
        /// </summary>
        /// <param name="data">Dictionary of data</param>
        /// <returns>List of kvps with data.</returns>
        private List<KeyValuePair<string, string>> FormatParams(Dictionary<string, string> data)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string,string>>();

            param.Add(new KeyValuePair<string, string>("api_key", this.apiKey));
            param.Add(new KeyValuePair<string, string>("api_uuid", this.uuid));
            param.Add(new KeyValuePair<string, string>("api_timestamp", Eta.GetTimestamp(DateTime.Now)));

            foreach (KeyValuePair<string, string> kvp in data)
            {
                param.Add(kvp);
            }

            return param;
        }

        /// <summary>
        /// Build parameters to send either in get or post.
        /// </summary>
        /// <param name="param">A formatted param list, as output by FormatParams</param>
        /// <returns>A string to be send as GET or POST data</returns>
        private string BuildParams(List<KeyValuePair<string, string>> param)
        {
            string data = "";
            string values = "";
            foreach (KeyValuePair<string, string> kvp in param)
            {
                data += kvp.Key + "=" + kvp.Value + "&";
                values += kvp.Value;
            }

            data.TrimEnd('&');

            if (this.apiSecret != "")
            {
                values += this.apiSecret;

                data += "checksum=" + MD5Core.GetHashString(values);
            }

            return data;
        }


        /// <summary>
        /// Gets the UNIX timestamp of a datetime object.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimestamp(DateTime time)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            int seconds = (int)time.Subtract(epoch).TotalSeconds;
            return seconds.ToString();
        }

        public static DateTime GetDateTime(string timestamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            int seconds = Convert.ToInt32(timestamp);
            return epoch.AddSeconds(seconds);
        }
        
        /* DEPRECATED */
        private string GetParam(List<KeyValuePair<string, string>> param, string key)
        {
            foreach (KeyValuePair<string, string> kvp in param)
            {
                if (kvp.Key == key)
                    return kvp.Value;
            }

            return null;
        }

        /// <summary>
        /// Gets a UUID. If none exits, create it.
        /// </summary>
        /// <returns></returns>
        private string GetUuid()
        {
            string sGuid;
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("etauuid", out sGuid))
            {
                Guid guid = System.Guid.NewGuid();

                StringBuilder sb = new StringBuilder();

                foreach (char c in guid.ToString())
                {
                    if (c != '-')
                    {
                        sb.Append(c);
                    }
                }

                sGuid = sb.ToString();
                IsolatedStorageSettings.ApplicationSettings.Add("etauuid", sGuid);
                IsolatedStorageSettings.ApplicationSettings.Save();
            }

            return sGuid;
        }

        public static void Noop()
        {
        }
    }
}
