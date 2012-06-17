using System.Json;
using Esmann.WP.Common.Json;


namespace EtaSDK.ApiModels
{
    public class Images
    {
        public string Thumb { get; set; }
        public string View { get; set; }
        public string Zoom { get; set; }

        public static Images FromJson(JsonValue item)
        {
            if (item.ContainsKey("images"))
            {
                Images images = new Images();
                var json = item.GetJsonValue(() => images);
                images.Thumb = json.GetJsonValue(() => images.Thumb);
                images.View = json.GetJsonValue(() => images.View);
                images.Zoom = json.GetJsonValue(() => images.Zoom);
                return images;
            }
            return null;
        }
    }
}
