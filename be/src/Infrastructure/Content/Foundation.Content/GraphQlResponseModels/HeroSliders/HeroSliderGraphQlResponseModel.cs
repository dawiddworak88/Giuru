using Foundation.Content.GraphQlResponseModels.Links;
using Foundation.Content.GraphQlResponseModels.Media;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Foundation.Content.GraphQlResponseModels.HeroSliders
{
    public class HeroSliderGraphQlResponseModel
    {
        [JsonProperty("heroSliderItems")]
        public IEnumerable<HeroSliderItemGraphQlResponseModel> HeroSliderItems { get; set; }
    }

    public class HeroSliderItemGraphQlResponseModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("link")]
        public LinkGraphQlResponseModel Link { get; set; }

        [JsonProperty("media")]
        public MediaGraphQlResponseModel Media { get; set; }
    }
}
