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

using System.Json;
using Esmann.WP.Common.Json;

namespace EtaSDK.ApiModels
{
    public class CatalogIndex
    {
        public string Id { get; set; }
        //public string PublicKey { get; set; }
        public string Page { get; set; }

        public static CatalogIndex FromJson(JsonValue item)
        {
            if (item.ContainsKey("catalog"))
            {
                CatalogIndex catalogIndex = new CatalogIndex();
                var json = item["catalog"];
                catalogIndex.Id = json.GetJsonValue(() => catalogIndex.Id);
                //catalogIndex.PublicKey = json.GetJsonValue(() => catalogIndex.PublicKey);
                catalogIndex.Page = json.GetJsonValue(() => catalogIndex.Page);
                return catalogIndex;
            }
            return null;
                 
        }

    }
}
