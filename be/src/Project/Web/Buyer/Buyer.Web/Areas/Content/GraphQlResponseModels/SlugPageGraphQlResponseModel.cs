using Buyer.Web.Areas.Content.Repositories;
using Foundation.PageContent.ResponseModels.Seo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.GraphQlResponseModels
{
    public class SlugPageGraphQlResponseModel
    {
        [JsonProperty("landingPages")]
        public LandingPage LandingPage { get; set; }
    }

    public class LandingPage
    {
        [JsonProperty("data")]
        public IEnumerable<Page> Data { get; set; }
    }

    public class Page
    {
        [JsonProperty("attributes")]
        public LandingPageAttributes Attributes { get; set; }
    }

    public class LandingPageAttributes
    {
        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("seo")]
        public Seo Seo { get; set; }

        [JsonConverter(typeof(BlockConverter))]
        public IEnumerable<Block> Blocks { get; set; }
    }

    public class Block
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    };

    public class ComponentSharedSlider : Block
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("navigation")]
        public bool HasNavigation { get; set; }

        [JsonProperty("skus")]
        public string Skus { get; set; }
    }

    public class ComponentSharedContent : Block
    {
        public IEnumerable<JObject> Content { get; set; }
    }

    public class ComponentBlocksVideo : Block
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("videoUrl")]
        public string VideoUrl { get; set; }

        [JsonProperty("video")]
        public ComponentBlocksInternalVideo Video { get; set; }
    }

    public class ComponentBlocksInternalVideo
    {
        [JsonProperty("data")]
        public ComponentBlocksInternalVideoData Data { get; set; }
    }

    public class ComponentBlocksInternalVideoData
    {
        [JsonProperty("attributes")]
        public ComponentBlocksInternalVideoAttributes Attributes { get; set; }
    }

    public class ComponentBlocksInternalVideoAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
