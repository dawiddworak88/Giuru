using Newtonsoft.Json;

namespace Buyer.Web.Shared.GraphQlResponseModels.Seo
{
    public class SeoGraphQlResponseModel
    {
        [JsonProperty("page")]
        public Page Page { get; set; }
    }

    public class Page
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public class Attributes 
    {
        [JsonProperty("seo")]
        public Seo Seo { get; set; }
    }

    public class Seo 
    {
        [JsonProperty("metaTitle")]
        public string MetaTitle { get; set; }

        [JsonProperty("metaDescription")]
        public string MetaDescription { get; set; }
    }
}
