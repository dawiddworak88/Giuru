using Buyer.Web.Shared.DomainModels.MainNavigationLinks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Buyer.Web.Shared.GraphQlResponseModels
{
    public class MainNavigationLinksGraphQlResponseModel
    {
        [JsonProperty("mainNavigation")]
        public MainNavigation MainNavigation { get; set; }
    }

    public class MainNavigation
    {
        [JsonProperty("data")]
        public MainNavigationData Data { get; set; }
    }

    public class MainNavigationData
    {
        [JsonProperty("attributes")]
        public MainNavigatioDataAttributes Attributes { get; set; }
    }

    public class MainNavigatioDataAttributes
    {
        [JsonProperty("mainNavigationLinks")]
        public MainNavigationDataAttibutesMainNavigationLinks MainNavigationLinks { get; set; }
    }

    public class MainNavigationDataAttibutesMainNavigationLinks
    {
        [JsonProperty("links")]
        public List<MainNavigationDataAttibutesMainNavigationLinksLink> Links {  get; set; }
    }

    public class MainNavigationDataAttibutesMainNavigationLinksLink
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