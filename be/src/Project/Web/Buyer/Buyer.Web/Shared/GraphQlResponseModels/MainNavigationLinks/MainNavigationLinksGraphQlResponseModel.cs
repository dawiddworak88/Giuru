using Newtonsoft.Json;
using System.Collections.Generic;
using Buyer.Web.Shared.GraphQlResponseModels.Shared;

namespace Buyer.Web.Shared.GraphQlResponseModels.MainNavigationLinks
{
    public class MainNavigationLinksGraphQlResponseModel
    {
        [JsonProperty("globalConfiguration")]
        public MainNavigationLinksComponent Component { get; set; }
    }

    public class MainNavigationLinksComponent
    {
        [JsonProperty("data")]
        public MainNavigationLinksData Data { get; set; }
    }

    public class MainNavigationLinksData
    {
        [JsonProperty("attributes")]
        public MainNavigationLinksAttributes Attributes { get; set; }
    }

    public class MainNavigationLinksAttributes
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
