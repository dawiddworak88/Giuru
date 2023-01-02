namespace Buyer.Web.Areas.Home.GraphQlResponseModels
{
    using Foundation.Content.GraphQlResponseModels.HeroSliders;
    using Newtonsoft.Json;

    public class HomePageHeroSliderGraphQlResponseModel
    {
        [JsonProperty("homePage")]
        public HomePage HomePage { get; set; }
    }

    public class HomePage
    {
        [JsonProperty("data")]
        public HomePageData Data { get; set; }
    }

    public class HomePageData
    {

        [JsonProperty("attributes")]
        public HomePageDataAttributes Attributes { get; set; }
    }

    public class HomePageDataAttributes
    {
        [JsonProperty("heroSlider")]
        public HeroSliderGraphQlResponseModel HeroSlider { get; set; }
    }
}
