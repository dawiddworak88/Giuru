using Newtonsoft.Json;
using System;

namespace Foundation.Content.GraphQlResponseModels.Links
{
    public partial class LinkGraphQlResponseModel
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("target")]
        public object Target { get; set; }
    }
}
