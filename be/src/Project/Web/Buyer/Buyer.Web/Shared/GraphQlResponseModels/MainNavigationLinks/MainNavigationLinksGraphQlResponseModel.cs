using Newtonsoft.Json;
using System.Collections.Generic;
using Buyer.Web.Shared.GraphQlResponseModels.Shared;

namespace Buyer.Web.Shared.GraphQlResponseModels.MainNavigationLinks
{
    public class MainNavigationLinksGraphQlResponseModel
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
        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public class Attributes
    {
        [JsonProperty("mainNavigationLinks")]
        public MainNavigationLinks MainNavigationLinks { get; set; }
    }

    public class MainNavigationLinks
    {
        [JsonProperty("links")]
        public List<Link> Links { get; set; }
    }
}
