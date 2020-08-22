using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HeroSliderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel>
    {
        private readonly IStringLocalizer<HeroSliderResources> heroSliderLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaService mediaService;

        public HeroSliderModelBuilder(IOptions<AppSettings> options,
            IMediaService mediaService,
            IStringLocalizer<HeroSliderResources> heroSliderLocalizer)
        {
            this.options = options;
            this.mediaService = mediaService;
            this.heroSliderLocalizer = heroSliderLocalizer;
        }

        public async Task<HeroSliderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new HeroSliderViewModel();

            var heroSliderItems = new List<HeroSliderItemViewModel>();

            var furnishLivingRoomHeroSliderItem = GetHeroSliderItem(this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.LivingRoomMediaId),
                this.heroSliderLocalizer["FurnishLivingRoom"],
                this.heroSliderLocalizer["FurnishLivingRoom"],
                this.heroSliderLocalizer["FurnishLivingRoom"],
                this.heroSliderLocalizer["FurnishLivingRoom"],
                this.heroSliderLocalizer["FurnishLivingRoom"],
                "#"
                );
            var furnishBedroomHeroSliderItem = GetHeroSliderItem(this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.BedroomMediaId),
                this.heroSliderLocalizer["DiscoverBedroom"],
                this.heroSliderLocalizer["DiscoverBedroom"],
                this.heroSliderLocalizer["DiscoverBedroom"],
                this.heroSliderLocalizer["DiscoverBedroom"],
                this.heroSliderLocalizer["DiscoverBedroomFurniture"],
                "#"
                );
            var furnishKidsRoomHeroSliderItem = GetHeroSliderItem(this.mediaService.GetMediaUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.KidsRoomMediaId),
                this.heroSliderLocalizer["ShopKidsRoom"],
                this.heroSliderLocalizer["ShopKidsRoom"],
                this.heroSliderLocalizer["ShopKidsRoom"],
                this.heroSliderLocalizer["ShopKidsRoom"],
                this.heroSliderLocalizer["ShopNow"],
                "#"
                );

            heroSliderItems.Add(furnishLivingRoomHeroSliderItem);
            heroSliderItems.Add(furnishBedroomHeroSliderItem);
            heroSliderItems.Add(furnishKidsRoomHeroSliderItem);

            viewModel.Items = heroSliderItems;

            return viewModel;
        }

        private HeroSliderItemViewModel GetHeroSliderItem(string imageSrc, 
            string imageAlt, 
            string imageTitle, 
            string teaserTitle, 
            string teaserText, 
            string ctaText, 
            string ctaUrl)
        { 
            return new HeroSliderItemViewModel
            {
                ImageSrc = imageSrc,
                ImageAlt = imageAlt,
                ImageTitle = imageTitle,
                TeaserTitle = teaserTitle,
                TeaserText = teaserText,
                CtaText = ctaText,
                CtaUrl = ctaUrl
            };
        }
    }
}
