using Foundation.PageContent.Components.Images;

namespace Foundation.PageContent.Components.HeroSliders.ViewModels
{
    public class HeroSliderItemViewModel
    {
        public string TeaserTitle { get; set; }
        public string TeaserText { get; set; }
        public string CtaUrl { get; set; }
        public string CtaText { get; set; }
        public ImageViewModel Image { get; set; }
    }
}
