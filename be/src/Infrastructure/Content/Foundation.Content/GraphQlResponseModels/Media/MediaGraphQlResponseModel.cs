using Newtonsoft.Json;
using System;

namespace Foundation.Content.GraphQlResponseModels.Media
{
    public class MediaGraphQlResponseModel
    {
        [JsonProperty("data")]
        public MediaData Data { get; set; }
    }

    public class MediaData
    {
        [JsonProperty("attributes")]
        public MediaDataAttributes Attributes { get; set; }
    }

    public class MediaDataAttributes
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("alternativeText")]
        public string AlternativeText { get; set; }
    }
}
