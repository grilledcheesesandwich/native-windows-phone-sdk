using System;
using System.Json;

namespace EtaSDK.ApiModels
{
    [Obsolete("is not used any more", true)]
    public class _Name
    {
        public string Local { get; set; }
        public string International { get; set; }

        public static _Name FromJson(JsonValue item)
        {
            if (item.ContainsKey("name"))
            {
                _Name name = new _Name();
                var json = item["name"];
                name.Local = json["local"];
                name.International = json["international"];
                return name;
            }
            return null;
        }
    }
}
