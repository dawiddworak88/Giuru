using Newtonsoft.Json;
using System.Collections.Generic;
using Buyer.Web.Shared.GraphQlResponseModels.Shared;

namespace Buyer.Web.Shared.GraphQlResponseModels.NotificationBar
{
    public class NotificationBarResponseModel
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
        [JsonProperty("notificationBar")]
        public NotificationBar NotificationBar { get; set; }
    }

    public class NotificationBar
    {
        [JsonProperty("notificationBarItem")]
        public IEnumerable<NotificationBarItem> Items { set; get; }
    }

    public class NotificationBarItem
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("link")]
        public Link Link { get; set; }
    }
}
