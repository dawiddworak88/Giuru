namespace Buyer.Web.Areas.Home.DomainModels
{
    public class HeroSliderItem
    {
        public string TeaserTitle { get; set; }
        public string TeaserText { get; set; }
        public string CtaUrl { get; set; }
        public string CtaText { get; set; }
        public HeroSliderItemImage Image { get; set; }
    }
}
