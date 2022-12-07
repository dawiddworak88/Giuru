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

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("cta")]
        public LinkGraphQlResponseModel Cta { get; set; }

        [JsonProperty("image")]
        public MediaGraphQlResponseModel Image { get; set; }
    }
}
