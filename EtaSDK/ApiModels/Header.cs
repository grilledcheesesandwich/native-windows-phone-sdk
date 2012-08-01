using System.Json;
using Esmann.WP.Common.Json;


namespace EtaSDK.ApiModels
{
    public class Header
    {
        public string Status { get; set; }
        public string Success { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public static Header FromJson(JsonValue item)
        {
            if (item != null)
            {
                var header = new Header();
                var json = item.JsonType == JsonType.String ? JsonValue.Parse(item) : item;
                header.Code = json.GetJsonValue(() => header.Code);
                header.Description = json.GetJsonValue(() => header.Description);
                header.Status = json.GetJsonValue(() => header.Status);
                header.Success = json.GetJsonValue(() => header.Success);
                return header;
            }
            else
            {
                return null;
            }
        }
    }
}
