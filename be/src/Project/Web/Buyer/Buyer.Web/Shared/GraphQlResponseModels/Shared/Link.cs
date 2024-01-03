using Newtonsoft.Json;

namespace Buyer.Web.Shared.GraphQlResponseModels.Shared
{
    public class Link
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("isExternal")]
        public bool IsExternal { get; set; }
    }
}
