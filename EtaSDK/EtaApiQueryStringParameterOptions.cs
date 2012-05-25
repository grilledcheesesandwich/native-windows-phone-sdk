using System;
using System.Collections.Generic;
using System.Text;
using EtaSDK.Properties;
using EtaSDK.Utils;

namespace EtaSDK
{
    public class EtaApiQueryStringParameterOptions
    {
        readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        public Dictionary<string, string> queryStringParams = new Dictionary<string, string>();
        public string responseType = "json";
        public string webMethod = "GET";
        public string contentType = "application/x-www-form-urlencoded";

        public EtaApiQueryStringParameterOptions()
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
        public string Uuid
        {
            get
            {
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

            sb = sb.Remove(sb.Length - 1, 1);

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

}
