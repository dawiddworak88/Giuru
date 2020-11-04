using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Shared.Configurations;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HeroSliderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel>
    {
        private readonly IStringLocalizer<HeroSliderResources> heroSliderLocalizer;
        private readonly IOptions<AppSettings> options;
        private readonly IMediaHelperService mediaService;
        private readonly LinkGenerator linkGenerator;

        public HeroSliderModelBuilder(IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            IStringLocalizer<HeroSliderResources> heroSliderLocalizer,
            LinkGenerator linkGenerator)
        {
            this.options = options;
            this.mediaService = mediaService;
            this.heroSliderLocalizer = heroSliderLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<HeroSliderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new HeroSliderViewModel();

            var heroSliderItems = new List<HeroSliderItemViewModel>();

            var furnishLivingRoomHeroSliderItem = GetHeroSliderItem(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.LivingRoomMediaId, true),
                this.heroSliderLocalizer["LowPrices"],
                this.heroSliderLocalizer["LowPrices"],
                this.heroSliderLocalizer["LowPrices"],
                string.Empty,
                this.heroSliderLocalizer["Discover"],
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Sectionals.Id }));

            var furnishBedroomHeroSliderItem = GetHeroSliderItem(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.BedroomMediaId, true),
                this.heroSliderLocalizer["TrackOrders"],
                this.heroSliderLocalizer["TrackOrders"],
                this.heroSliderLocalizer["TrackOrders"],
                string.Empty,
                this.heroSliderLocalizer["Browse"],
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Beds.Id }));

            var furnishKidsRoomHeroSliderItem = GetHeroSliderItem(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, HeroSliderItemConstants.Media.KidsRoomMediaId, true),
                this.heroSliderLocalizer["EasyRepurchase"],
                this.heroSliderLocalizer["EasyRepurchase"],
                this.heroSliderLocalizer["EasyRepurchase"],
                string.Empty,
                this.heroSliderLocalizer["Shop"],
                this.linkGenerator.GetPathByAction("Index", "Category", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, HeroSliderItemConstants.Categories.Sectionals.Id }));

            heroSliderItems.Add(furnishBedroomHeroSliderItem);
            heroSliderItems.Add(furnishLivingRoomHeroSliderItem);
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
