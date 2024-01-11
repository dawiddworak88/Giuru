using Newtonsoft.Json;
using System.Collections.Generic;
using Buyer.Web.Shared.GraphQlResponseModels.Shared;

namespace Buyer.Web.Shared.GraphQlResponseModels.NotificationBar
{
    public class NotificationBarResponseModel
    {
        [JsonProperty("globalConfiguration")]
        public NotificationBarComponent Component { get; set; } 
    }

    public class NotificationBarComponent
    {
        [JsonProperty("data")]
        public NotificationBarData Data { get; set; }
    }

    public class NotificationBarData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributes")]
        public NotificationBarAttributes Attributes { get; set; }
    }

    public class NotificationBarAttributes
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
